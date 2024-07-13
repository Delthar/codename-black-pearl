using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public event EventHandler<OnVelocityChangedEventArgs> OnVelocityChanged;
    public class OnVelocityChangedEventArgs : EventArgs
    {
        public float maxVelocity;
        public float oldVelocity;
        public float newVelocity;
        public float rigidbodySpeed;
    }

    [SerializeField] private float accelerationSpeed = 5;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float maxRotationSpeed = 10f;
    [SerializeField] private float maxAccelerationSpeed = 10f;
    [SerializeField] private float decelerateSpeedOnHit = 10f;
    [SerializeField, Range(0, 0.9f)] private float windImpactStrength = 0.45f;

    private Rigidbody2D rb;
    private float forwardVelocity;
    private float angularVelocity;

    private float currentForwardVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D> ();
    }

    private void Start() 
    {
        UpdateCurrentVelocity();
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
        
        // Angular Velocity
        angularVelocity = Mathf.Clamp(angularVelocity, -maxRotationSpeed, maxRotationSpeed);
        rb.angularVelocity = forwardVelocity >= 0 ? angularVelocity : -angularVelocity ;

        // Forward Velocity
        forwardVelocity = Mathf.Clamp(forwardVelocity, -maxAccelerationSpeed * 0.25f, maxAccelerationSpeed);
        Vector2 windDirectionNormalized = WindController.Instance.GetWindDirectionNormalized();
        float windForce = Vector2.Dot(transform.up, windDirectionNormalized) * WindController.Instance.GetWindForce();
        float windForceDependent = windForce * Mathf.InverseLerp(0, maxAccelerationSpeed, forwardVelocity);
        Vector3 forwardInput = transform.up * (forwardVelocity + (windForceDependent * windImpactStrength));
        rb.velocity = forwardInput;
        UpdateCurrentVelocity();

    }

    private void UpdateCurrentVelocity()
    {
        OnVelocityChanged?.Invoke(this, new OnVelocityChangedEventArgs { oldVelocity = currentForwardVelocity, newVelocity = forwardVelocity, maxVelocity = maxAccelerationSpeed, rigidbodySpeed = rb.velocity.magnitude });
        currentForwardVelocity = forwardVelocity; 
    }
}
