using UnityEngine;

public interface IFireable
{
    public abstract void Fire(Vector2 direction, float force, float distance);
}