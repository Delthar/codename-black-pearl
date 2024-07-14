using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AITest : MonoBehaviour
{
    [SerializeField] private Transform goHereTransform;
    private ShipController shipController;

    private void Awake()
    {
        shipController = GetComponent<ShipController>();
    }

    private void Update()
    {
        Vector3 direction = transform.InverseTransformDirection(goHereTransform.position - transform.position);
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
    }
}
