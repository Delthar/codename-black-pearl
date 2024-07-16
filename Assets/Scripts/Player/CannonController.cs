using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The reference to the left cannon")]
    [SerializeField] private GameObject leftCannon;
    [Tooltip("The reference to the right cannon")]
    [SerializeField] private GameObject rightCannon;
    [Tooltip("The reference to the LineRenderer of the GameObject")]
    [SerializeField] private LineRenderer chargeLine;

    [Header("Cannon Parameters")]
    [Tooltip("The angle a cannon can rotate to")]
    [Range(15, 75)]
    [SerializeField] private float cannonAngle;
    [Tooltip("The upper limit of charge that determines the maximum range of a shot")]
    [Range(16, 20)]
    [SerializeField] private float maximumCharge;
    [Tooltip("The lower limit of charge that determines the minimum range of a shot")]
    [Range(4, 12)]
    [SerializeField] private float minimumCharge;
    [Tooltip("The rate at which charge is accumulated")]
    [Range(12, 16)]
    [SerializeField] private float chargeMultiplier;
    [Tooltip("The amount of force of a shot")]
    [Range(8, 18)]
    [SerializeField] private float fireForce;
    [Tooltip("The cooldown before another shot can be made")]
    [Range(2, 8)]
    [SerializeField] private float fireCooldown;

    [Header("Cannon Status")]
    [Tooltip("The current charged amount that determines the range of the cannonball")]
    [Range(4, 20)]
    [SerializeField] private float currentCharge;
    [Tooltip("The current cooldown before another shot can be made")]
    [Range(0, 8)]
    [SerializeField] private float currentFireCooldown;

    #if UNITY_EDITOR

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(rightCannon.transform.position, CalculateAngleDirectionTarget(rightCannon.transform, cannonAngle, maximumCharge));    
        Gizmos.DrawLine(rightCannon.transform.position, CalculateAngleDirectionTarget(rightCannon.transform, -cannonAngle, maximumCharge));    
        Gizmos.DrawLine(leftCannon.transform.position, CalculateAngleDirectionTarget(leftCannon.transform, cannonAngle, maximumCharge));    
        Gizmos.DrawLine(leftCannon.transform.position, CalculateAngleDirectionTarget(leftCannon.transform, -cannonAngle, maximumCharge));    
    }
    
    #endif


    private void Update() 
    {
        Aim();
    }

    private void Aim()
    {
        if (currentFireCooldown <= 0)
        {
            (float angle, Vector3 targetDirection) rightCannonAngleDirection = CalculateAngleDirectionAndTarget(rightCannon.transform);
            (float angle, Vector3 targetDirection) leftCannonAngleDirection = CalculateAngleDirectionAndTarget(leftCannon.transform);

            if (PlayerInput.Instance.GetFireAction().IsPressed())
            {
                if (currentCharge < maximumCharge) currentCharge += Time.deltaTime * chargeMultiplier;

                if (-cannonAngle <= rightCannonAngleDirection.angle && rightCannonAngleDirection.angle <= cannonAngle)
                {
                    DrawChargeLine(rightCannon.transform, rightCannon.transform.position + rightCannonAngleDirection.targetDirection * currentCharge, Color.red);    
                }
                else if (-cannonAngle <= leftCannonAngleDirection.angle && leftCannonAngleDirection.angle <= cannonAngle)
                {
                    DrawChargeLine(leftCannon.transform, leftCannon.transform.position + leftCannonAngleDirection.targetDirection * currentCharge, Color.red);            
                }
                else
                {
                    DrawChargeLine(transform, transform.position, Color.white);
                }
            }
            else if (PlayerInput.Instance.GetFireAction().WasReleasedThisFrame())
            {
                if (-cannonAngle <= rightCannonAngleDirection.angle && rightCannonAngleDirection.angle <= cannonAngle)
                {
                    Fire(rightCannon.transform, rightCannonAngleDirection.targetDirection);
                }
                else if (-cannonAngle <= leftCannonAngleDirection.angle && leftCannonAngleDirection.angle <= cannonAngle)
                {
                    Fire(leftCannon.transform, leftCannonAngleDirection.targetDirection);
                }

                ResetCharge();
                ResetChargeLine();
                ResetFireCooldown();
            }
        }
        else if (currentFireCooldown > 0) currentFireCooldown -= Time.deltaTime;
    }

    private (float, Vector3) CalculateAngleDirectionAndTarget(Transform relationPoint)
    {
        Vector3 worldDirection = GetMouseDirection(relationPoint);
        Vector3 localDirection = relationPoint.InverseTransformDirection(worldDirection).normalized;
        float angle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;
        Vector3 targetDirection = relationPoint.rotation * new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

        return (angle, targetDirection);
    }

    private Vector3 CalculateAngleDirectionTarget(Transform relationPoint, float angle, float range) => relationPoint.position + relationPoint.rotation * new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * range;  
    
    private Vector3 GetMouseDirection(Transform relationPoint)
    {
        Vector3 direction = Utils.GetMouseWorldPosition() - relationPoint.position;
        direction.z = 0;
        return direction.normalized;
    }

    private void DrawChargeLine(Transform relationPoint, Vector3 targetDirection, Color lineColor)
    {
        chargeLine.SetPosition(0, new Vector3(relationPoint.position.x, relationPoint.position.y, 0));
        chargeLine.SetPosition(1, targetDirection);
        chargeLine.startColor = lineColor;
        chargeLine.endColor = lineColor;
    }

    private void ResetChargeLine()
    {
        chargeLine.SetPosition(0, transform.position);
        chargeLine.SetPosition(1, transform.position);
    }

    private void ResetCharge() => currentCharge = minimumCharge;

    private void ResetFireCooldown() => currentFireCooldown = fireCooldown;

    private void Fire(Transform relationPoint, Vector2 targetDirection)
    {
        GameObject cannonball = ObjectPool.Instance.GetPoolObject();

        cannonball.GetComponent<IPoolable>().Initialize(relationPoint);
        cannonball.GetComponent<IFireable>().Fire(gameObject, targetDirection, fireForce, currentCharge);   
    }
}
