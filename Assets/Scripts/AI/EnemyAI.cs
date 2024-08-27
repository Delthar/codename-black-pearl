using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyState currentEnemyState = EnemyState.Idle;
    [SerializeField, Range(1, 10)] private float avoidanceDistance = 4;
    [SerializeField] private LayerMask avoidanceLayerMask = new LayerMask();

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
