using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AITest : MonoBehaviour
{
    [SerializeField] private Transform currentTargetTransform;
    [SerializeField] private Vector2 currentTargetPosition = Vector2.zero;
    private ShipController shipController;
    private Pathfinding pathfinding;
    List<PathNode> pathfindingPath = new List<PathNode>();

    private void Awake()
    {
        pathfinding = new Pathfinding();
        shipController = GetComponent<ShipController>();
    }

    private void Update()
    {
        if(currentTargetTransform != null)
        {
            var start = PathNetwork.Instance.FindClosestNodeFromPosition(transform.position);
            var end = PathNetwork.Instance.FindClosestNodeFromPosition(currentTargetTransform.position);
            pathfindingPath = pathfinding.FindPath(start, end);
            if(pathfindingPath.Count > 0)
            {
                currentTargetPosition = pathfindingPath.FirstOrDefault().transform.position;
            }
        }

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

    private void OnDrawGizmos()
    {
        if(shipController != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            Handles.Label(transform.position,  $"Forward Velocity: {shipController.GetForwardVelocity()}\nAngular Velocity: {shipController.GetAngularVelocity()}", style);
        }

        int pathCount = pathfindingPath.Count;
        for(int i = 0; i < pathCount; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pathfindingPath[i].transform.position, 0.5f);
            if(i < (pathCount - 1))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(pathfindingPath[i].transform.position, pathfindingPath[i + 1].transform.position);
            }
        }
    }
}
