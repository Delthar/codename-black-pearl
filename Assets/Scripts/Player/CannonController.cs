using UnityEditor.EditorTools;
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
        float angle = CalculateAngle();
        Debug.Log(angle);
        
        if (PlayerInput.Instance.GetFireAction().WasPressedThisFrame())
        {
            if (angle > upperAngle && angle < lowerAngle)
            {
                Debug.Log("Right");
                Fire(rightCannon.transform);
            }
            else if (angle < -upperAngle && angle > -lowerAngle)
            {
                Debug.Log("Left");
                Fire(leftCannon.transform);
            }
            else
            {
                Debug.Log("Deadzone");
            }
        }
    }

    private void Fire(Transform cannonPosition)
    {
        GameObject cannonball = ObjectPool.Instance.GetPoolObject();
        cannonball.GetComponent<IPoolable>().Initialize(cannonPosition);
        cannonball.GetComponent<IFireable>().Fire(new Vector2(10, 10));   
    }

    private float CalculateAngle()
    {
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        Vector3 worldDirection = cursorWorldPosition - transform.position;
        Vector3 localDirection = transform.InverseTransformDirection(worldDirection);

        return Mathf.Atan2(localDirection.x, localDirection.y) * Mathf.Rad2Deg;
    }

}
