using UnityEngine;

public class Cannonball : MonoBehaviour, IPoolable, IFireable
{
    private Rigidbody2D body;


    public void Initialize(Transform initializationPoint)
    {
        transform.position = initializationPoint.position;
        transform.rotation = initializationPoint.rotation;
    }

    public void Fire(Vector2 direction) => body.AddForce(direction, ForceMode2D.Impulse);
}