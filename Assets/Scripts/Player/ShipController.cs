using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float accelerationSpeed = 5;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float maxRotationSpeed = 10f;
    [SerializeField] private float maxAccelerationSpeed = 10f;
    [SerializeField] private float decelerateSpeedOnHit = 10f;

    private Rigidbody2D rb;
    private float forwardVelocity;
    private float angularVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D> ();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        forwardVelocity -= (forwardVelocity > 0 ? Time.deltaTime : -Time.deltaTime) * decelerateSpeedOnHit;
    }

    
    private void Update()
    {
        Vector2 input = PlayerInput.Instance.GetMoveInput();
        
        angularVelocity -= input.x * Time.deltaTime * rotationSpeed;
        forwardVelocity += input.y * Time.deltaTime * accelerationSpeed;

        if(input.x == 0)
        {
            angularVelocity -= (angularVelocity > 0 ? Time.deltaTime : -Time.deltaTime) * rotationSpeed;
        }

        angularVelocity = Mathf.Clamp(angularVelocity, -maxRotationSpeed, maxRotationSpeed);
        forwardVelocity = Mathf.Clamp(forwardVelocity, -maxAccelerationSpeed * 0.25f, maxAccelerationSpeed);

        float windForce = Vector2.Dot(transform.up, WindController.Instance.GetWindDirection()) * WindController.Instance.GetWindForce();

        // TODO: Add Wind Force
        Vector3 forwardInput = transform.up * forwardVelocity;
        rb.velocity = forwardInput;
        rb.angularVelocity = forwardVelocity >= 0 ? angularVelocity : -angularVelocity ;


        
    }
}
