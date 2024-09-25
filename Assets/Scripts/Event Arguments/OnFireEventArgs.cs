using System;
using UnityEngine;

public class OnFireEventArgs : EventArgs
{
    public Cannon cannon;
    public GameObject instigator;
    public Vector3 position;
    public float force;
    public float charge;

    public OnFireEventArgs(Cannon cannon, GameObject instigator, Vector3 position, float force, float charge)
    {
        this.cannon = cannon;
        this.instigator = instigator;
        this.position = position;
        this.force = force;
        this.charge = charge;
    }
}