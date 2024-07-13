using UnityEditor.EditorTools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The reference to the left cannon")]
    [SerializeField] private GameObject leftCannon;
    [Tooltip("The reference to the right cannon")]
    [SerializeField] private GameObject rightCannon;

    [Header("Cannon Parameters")]
    [Tooltip("The angle a cannon can rotate to at the front of the ship")]
    [Range(0, 90)]
    [SerializeField] private float upperAngle;
    [Tooltip("The angle a cannon can rotate to at the back of the ship")]
    [Range(90, 180)]
    [SerializeField] private float lowerAngle;
    [Tooltip("The upper limit of charge that determines the maximum range of a shot")]
    [Range(5, 20)]
    [SerializeField] private float maximumCharge;
    [Tooltip("The lower limit of charge that determines the minimum range of a shot")]
    [Range(5, 20)]
    [SerializeField] private float minimumCharge;
    [Tooltip("The rate at which the charge amount is accumulated")]
    [Range(6, 12)]
    [SerializeField] private float chargeMultiplier;
    [Tooltip("The amount of force of a shot")]
    [Range(8, 24)]
    [SerializeField] private float fireForce;

    [Header("Cannon Status")]
    [Tooltip("The charged amount that determines the range of the cannonball")]
    [Range(5, 20)]
    [SerializeField] private float chargeAmount;
    

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

        if (PlayerInput.Instance.GetFireAction().IsPressed())
        {
            if (chargeAmount < maximumCharge)
            {
                if (rightCannonAngleDirection.angle > upperAngle && rightCannonAngleDirection.angle < lowerAngle)
                {
                    chargeAmount += Time.deltaTime * chargeMultiplier;    
                }
                else if (leftCannonAngleDirection.angle < -upperAngle && leftCannonAngleDirection.angle > -lowerAngle)
                {
                    chargeAmount += Time.deltaTime * chargeMultiplier;
                }
            }
        }
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

            ResetCharge();
        }
    }

    private void Fire(Transform cannonPosition, Vector2 direction)
    {
        GameObject cannonball = ObjectPool.Instance.GetPoolObject();
        cannonball.GetComponent<IPoolable>().Initialize(cannonPosition);
        cannonball.GetComponent<IFireable>().Fire(direction, fireForce, chargeAmount);   
    }

    private (float, Vector3) CalculateAngleDirection(Transform cannon)
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 worldDirection = cursorWorldPosition - cannon.position;
        Vector3 localDirection = transform.InverseTransformDirection(worldDirection);

        return (Mathf.Atan2(localDirection.x, localDirection.y) * Mathf.Rad2Deg, worldDirection);
    }

    private void ResetCharge() => chargeAmount = minimumCharge;
}
