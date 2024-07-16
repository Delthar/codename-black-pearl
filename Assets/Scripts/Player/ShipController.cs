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
    private Vector2 currentInput;
    private float currentForwardVelocity;
    private bool controllerEnabled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D> ();
    }

    private void Start() 
    {
        controllerEnabled = true;
        UpdateCurrentVelocity();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // forwardVelocity -= (forwardVelocity > 0 ? Time.deltaTime : -Time.deltaTime) * decelerateSpeedOnHit;
        forwardVelocity -= forwardVelocity > 0 ? forwardVelocity * 0.5f : -forwardVelocity * 0.5f;
    }

    private void Update()
    {
        if (!controllerEnabled)
            return;
        
        angularVelocity -= currentInput.x * Time.deltaTime * rotationSpeed;
        forwardVelocity += currentInput.y * Time.deltaTime * accelerationSpeed;

        if(currentInput.x == 0)
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

        currentInput = Vector2.zero;
    }

    private void UpdateCurrentVelocity()
    {
        OnVelocityChanged?.Invoke(this, new OnVelocityChangedEventArgs { oldVelocity = currentForwardVelocity, newVelocity = forwardVelocity, maxVelocity = maxAccelerationSpeed, rigidbodySpeed = rb.velocity.magnitude });
        currentForwardVelocity = forwardVelocity; 
    }

    public float GetMaxAccelerationSpeed() => maxAccelerationSpeed;
    public float GetForwardVelocity() => forwardVelocity;
    public float GetAngularVelocity() => angularVelocity;
    public void SetForwardInput(float value) => currentInput.y = value;
    public void SetAngularInput(float value) => currentInput.x = value;
    public void Disable() => controllerEnabled = false;
    public void Enable() => controllerEnabled = true;
    public void Stop() 
    {
        forwardVelocity = 0;
        angularVelocity = 0;
    } 
        
    
}
