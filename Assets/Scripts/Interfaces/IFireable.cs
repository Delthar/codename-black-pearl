using UnityEngine;

/// <summary>
/// Enables an object the ability to be fired.
/// </summary>
public interface IFireable
{
    /// <summary>
    /// Fires the object with the specified parameters.
    /// </summary>
    /// <param name="instigator">The object that initiated the firing action.</param>
    /// <param name="direction">The direction in which the object travels.</param>
    /// <param name="force">The force applied to the object.</param>
    /// <param name="distance">The distance the object travels in the given direction.</param>
    public abstract void Fire(GameObject instigator, Vector2 direction, float force, float distance);
}