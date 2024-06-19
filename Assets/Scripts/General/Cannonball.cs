using UnityEngine;

public class Cannonball : MonoBehaviour, IPoolable, IFireable
{
    private Rigidbody2D body;
    
    [SerializeField] private float maxLifeTime = 3;
    [SerializeField] private float maxDistance = 20;
    [SerializeField] private float defaultScale = 1;
    [SerializeField] private AnimationCurve scaleCurve = new AnimationCurve();

    private float lifetime = 0;
    private Vector2 spawnPosition;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        float distanceTraveled = Vector3.Distance(spawnPosition, transform.position);
        float animCurve = Mathf.InverseLerp(0, maxDistance, distanceTraveled);
        float animCurveValidate = scaleCurve.Evaluate(animCurve) + defaultScale;
        transform.localScale = new Vector3(animCurveValidate, animCurveValidate, animCurveValidate);

        if(lifetime >= maxLifeTime || distanceTraveled > maxDistance)
        {
            lifetime = 0;
            ObjectPool.Instance.ReturnPoolObject(gameObject);
        }
    }

    public void Initialize(Transform initializationPoint)
    {
        spawnPosition = initializationPoint.position;
        transform.position = initializationPoint.position;
        transform.rotation = initializationPoint.rotation;
    }

    public void Fire(Vector2 direction, float force) => body.AddForce(direction.normalized * force, ForceMode2D.Impulse);
}