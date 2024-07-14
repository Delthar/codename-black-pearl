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
    [Tooltip("The angle a cannon can rotate to at the front of the ship")]
    [Range(0, 90)]
    [SerializeField] private float upperAngle;
    [Tooltip("The angle a cannon can rotate to at the back of the ship")]
    [Range(90, 180)]
    [SerializeField] private float lowerAngle;
    [Tooltip("The upper limit of charge that determines the maximum range of a shot")]
    [Range(16, 24)]
    [SerializeField] private float maximumCharge;
    [Tooltip("The lower limit of charge that determines the minimum range of a shot")]
    [Range(4, 16)]
    [SerializeField] private float minimumCharge;
    [Tooltip("The rate at which the charge amount is accumulated")]
    [Range(12, 16)]
    [SerializeField] private float chargeMultiplier;
    [Tooltip("The amount of force of a shot")]
    [Range(8, 18)]
    [SerializeField] private float fireForce;

    [Header("Cannon Status")]
    [Tooltip("The charged amount that determines the range of the cannonball")]
    [Range(4, 24)]
    [SerializeField] private float chargeAmount;

    private void Update() 
    {
        Aim(); 
    }


    private void Aim()
    {
        (float angle, Vector3 worldDirection, Vector3 localDirection) rightCannonAngleDirection = CalculateAngleDirection(rightCannon.transform);
        (float angle, Vector3 worldDirection, Vector3 localDirection) leftCannonAngleDirection = CalculateAngleDirection(leftCannon.transform);

        if (PlayerInput.Instance.GetFireAction().IsPressed())
        {
            if (chargeAmount < maximumCharge) chargeAmount += Time.deltaTime * chargeMultiplier;

            if (rightCannonAngleDirection.angle > upperAngle && rightCannonAngleDirection.angle < lowerAngle)
            {
                DrawChargeLine(rightCannon.transform.localPosition, rightCannonAngleDirection.worldDirection.normalized * chargeAmount);    
            }
            else if (leftCannonAngleDirection.angle < -upperAngle && leftCannonAngleDirection.angle > -lowerAngle)
            {
                DrawChargeLine(leftCannon.transform.localPosition, leftCannonAngleDirection.worldDirection.normalized * chargeAmount);
            }
        }
        else if (PlayerInput.Instance.GetFireAction().WasReleasedThisFrame())
        {
            if (rightCannonAngleDirection.angle > upperAngle && rightCannonAngleDirection.angle < lowerAngle)
            {
                Fire(rightCannon.transform, rightCannonAngleDirection.worldDirection.normalized);
            }
            else if (leftCannonAngleDirection.angle < -upperAngle && leftCannonAngleDirection.angle > -lowerAngle)
            {
                Fire(leftCannon.transform, leftCannonAngleDirection.worldDirection.normalized);
            }

            ResetCharge();
            ResetChargeLine();
        }
    }

    private void Fire(Transform cannonPosition, Vector2 direction)
    {
        GameObject cannonball = ObjectPool.Instance.GetPoolObject();
        cannonball.GetComponent<IPoolable>().Initialize(cannonPosition);
        cannonball.GetComponent<IFireable>().Fire(gameObject, direction, fireForce, chargeAmount);   
    }

    private (float, Vector3, Vector3) CalculateAngleDirection(Transform relativePosition)
    {
        (Vector3 localDirection, Vector3 worldDirection) = GetMouseDirection(relativePosition);

        return (Mathf.Atan2(localDirection.x, localDirection.y) * Mathf.Rad2Deg, worldDirection, localDirection);
    }

    private (Vector3, Vector3) GetMouseDirection(Transform relativePosition)
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 worldDirection = cursorWorldPosition - relativePosition.position;
        Vector3 localDirection = transform.InverseTransformDirection(worldDirection);

        return (worldDirection, localDirection);
    }

    private void DrawChargeLine(Vector3 cannonPosition, Vector3 targetPosition)
    {
        chargeLine.SetPosition(0, new Vector3(cannonPosition.x, cannonPosition.y,0));
        chargeLine.SetPosition(1, new Vector3(targetPosition.x, targetPosition.y, 0));
    }

    private void ResetChargeLine()
    {
        chargeLine.SetPosition(0, new Vector3(0, 0, 0));
        chargeLine.SetPosition(1, new Vector3(0, 0, 0));
    }

    private void ResetCharge() => chargeAmount = minimumCharge;
}
