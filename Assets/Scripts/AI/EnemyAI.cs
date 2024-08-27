using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform currentTargetTransform;
    [SerializeField] private Vector2 currentTargetPosition = Vector2.zero;
    [SerializeField] private EnemyState currentEnemyState = EnemyState.Idle;
    [SerializeField, Range(1, 10)] private float avoidanceDistance = 4;
    [SerializeField] private LayerMask avoidanceLayerMask = new LayerMask();

    private ShipController shipController;
    private bool isColliding;
    private Vector2 collidingPosition;
    private Vector2 collidingNormal;

    private enum EnemyState
    {
        Idle,
        Searching,
        CollisionAvoiding,
        Chasing,
        Flee
    }

    private void Awake()
    {
        // pathfinding = new Pathfinding();
        shipController = GetComponent<ShipController>();
    }

    void Update()
    {
        currentTargetPosition = (Vector2)transform.position * transform.up * avoidanceDistance;

        if (currentTargetPosition == Vector2.zero)
            return;
        
        Vector3 direction = transform.InverseTransformDirection(currentTargetPosition - (Vector2)transform.position);
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float rotValue = 0;
        if (angle > 10)
        {
            rotValue = Mathf.InverseLerp(10, 45, angle);
            if (shipController.GetForwardVelocity() > (shipController.GetMaxAccelerationSpeed() * 0.5f))
            {
                shipController.SetForwardInput(-Mathf.InverseLerp(1, 180, angle));
            }
            else shipController.SetForwardInput(1);
        }
        else if (angle < -10)
        {
            rotValue = -Mathf.InverseLerp(-10, -45, angle);
            if (shipController.GetForwardVelocity() > (shipController.GetMaxAccelerationSpeed() * 0.5f))
            {
                shipController.SetForwardInput(-Mathf.InverseLerp(-1, -180, angle));
            }
            else shipController.SetForwardInput(1);
        }
        else
        {
            shipController.SetForwardInput(1);
        }
        shipController.SetAngularInput(rotValue);
    }

    private void FixedUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.up * avoidanceDistance, avoidanceDistance, avoidanceLayerMask);
        isColliding = raycastHit;
        if(isColliding)
        {
            collidingPosition = raycastHit.point;
            collidingNormal = raycastHit.normal;
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = isColliding ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, transform.up * (isColliding ? ((Vector2)transform.position - collidingPosition).magnitude : avoidanceDistance));

        if(isColliding)
        {
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(collidingPosition, Vector2.Reflect(transform.up, collidingNormal));
        }
    }




}
