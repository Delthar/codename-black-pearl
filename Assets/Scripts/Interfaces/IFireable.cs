using UnityEngine;

public interface IFireable
{
    public abstract void Fire(GameObject shooter, Vector2 direction, float force, float distance);
}