using UnityEditor.EditorTools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The reference to the left cannon GameObject")]
    [SerializeField] private GameObject leftCannon;
    [Tooltip("The reference to the right cannon GamePbject")]
    [SerializeField] private GameObject rightCannon;

    [Header("Cannon Parameters")]
    [Tooltip("The angle a cannon can rotate to at the front of the ship")]
    [Range(0, 90)]
    [SerializeField] private float upperAngle;
    [Tooltip("The angle a cannon can rotate to at the back of the ship")]
    [Range(90, 180)]
    [SerializeField] private float lowerAngle;
    [Tooltip("The maximum amount of distance a cannonball can be shot to")]
    [Range(5, 20)]
    [SerializeField] private float maximumFireDistance;
    [Tooltip("The amount of force a cannonball is shot with")]
    [Range(1, 30)]
    [SerializeField] private float fireForce;
    [Tooltip("The amount of force a cannonball is shot with")]
    [Range(4, 8)]
    [SerializeField] private float chargeMultiplier;

    [Header("Cannon Status")]
    [Tooltip("The amount of force a cannonball is shot to")]
    [Range(5, 20)]
    [SerializeField] private float currentFireDistance;
    
    private void Awake()
    {
        upperAngle = 15;
        lowerAngle = 165;    
    }

    private void Update() 
    {
        Aim(); 
    }

    private void Aim()
    {
        (float angle, Vector3 direction) rightCannonAngleDirection = CalculateAngleDirection(rightCannon.transform);
        (float angle, Vector3 direction) leftCannonAngleDirection = CalculateAngleDirection(leftCannon.transform);

        // Start
        if(PlayerInput.Instance.GetFireAction().WasPressedThisFrame())
        {
            currentFireDistance = 5;
        }
        // Update
        else if (PlayerInput.Instance.GetFireAction().IsPressed())
        {
            if (currentFireDistance < maximumFireDistance)
            {
                if (rightCannonAngleDirection.angle > upperAngle && rightCannonAngleDirection.angle < lowerAngle)
                {
                    currentFireDistance += Time.deltaTime * chargeMultiplier;    
                }
                else if (leftCannonAngleDirection.angle < -upperAngle && leftCannonAngleDirection.angle > -lowerAngle)
                {
                    currentFireDistance += Time.deltaTime * chargeMultiplier;
                }
            }
        }
        // Stop
        else if (PlayerInput.Instance.GetFireAction().WasReleasedThisFrame())
        {
            if (rightCannonAngleDirection.angle > upperAngle && rightCannonAngleDirection.angle < lowerAngle)
            {
                Fire(rightCannon.transform, rightCannonAngleDirection.direction.normalized);
            }
            else if (leftCannonAngleDirection.angle < -upperAngle && leftCannonAngleDirection.angle > -lowerAngle)
            {
                Fire(leftCannon.transform, leftCannonAngleDirection.direction.normalized);
            }

            currentFireDistance = 5;
        }
    }

    private void Fire(Transform cannonPosition, Vector2 direction)
    {
        GameObject cannonball = ObjectPool.Instance.GetPoolObject();
        cannonball.GetComponent<IPoolable>().Initialize(cannonPosition);
        cannonball.GetComponent<IFireable>().Fire(direction, fireForce, currentFireDistance, gameObject);   
    }

    private (float, Vector3) CalculateAngleDirection(Transform transform)
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 worldDirection = cursorWorldPosition - transform.position;
        Vector3 localDirection = this.transform.InverseTransformDirection(worldDirection);

        return (Mathf.Atan2(localDirection.x, localDirection.y) * Mathf.Rad2Deg, worldDirection);
    }

}
