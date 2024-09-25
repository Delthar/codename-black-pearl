using System;
using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Static Event")]
    [Tooltip("Triggers whenever a shot is fired by a cannon")]
    [SerializeField] public static EventHandler<OnCannonFireEventArgs> OnCannonFire;

    public void CannonController_OnFire(object sender, OnFireEventArgs e)
    {
        // if(this != e.cannon)
        //     return;

       
    }

    public void Fire(OnFireEventArgs e)
    {

        GameObject cannonball = ObjectPool.Instance.GetPoolObject();
        Vector2 fireDirection = (e.position - transform.position).normalized;
        Debug.DrawRay(transform.position, fireDirection, Color.red, 20f);

        cannonball.GetComponent<IPoolable>().Initialize(transform);
        cannonball.GetComponent<IFireable>().Fire(e.instigator, fireDirection, e.force, e.charge);

        InvokeCannonFire();
    }

    private void InvokeCannonFire() => OnCannonFire?.Invoke(this, new OnCannonFireEventArgs(transform));
}