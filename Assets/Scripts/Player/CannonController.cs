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

    [SerializeField] private float fireForce = 30;

    private float defaultRightCanonRotation;
    private float defaultLeftCanonRotation;
    
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
        (float, Vector3) data1 = CalculateAngle(rightCannon.transform);
        (float, Vector3) data2 = CalculateAngle(leftCannon.transform);
        if (PlayerInput.Instance.GetFireAction().WasPressedThisFrame())
        {
            if (data1.Item1 > upperAngle && data1.Item1 < lowerAngle)
            {
                Fire(rightCannon.transform, data1.Item2.normalized);
            }
            else if (data2.Item1 < -upperAngle && data2.Item1 > -lowerAngle)
            {
                Fire(leftCannon.transform, data2.Item2.normalized);
            }
        }
    }

    private void Fire(Transform cannonPosition, Vector2 direction)
    {
        GameObject cannonball = ObjectPool.Instance.GetPoolObject();
        cannonball.GetComponent<IPoolable>().Initialize(cannonPosition);
        cannonball.GetComponent<IFireable>().Fire(direction, fireForce);   
    }

    private (float, Vector3) CalculateAngle(Transform transform)
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 worldDirection = cursorWorldPosition - transform.position;
        Vector3 localDirection = this.transform.InverseTransformDirection(worldDirection);

        return (Mathf.Atan2(localDirection.x, localDirection.y) * Mathf.Rad2Deg, worldDirection);
    }

}
