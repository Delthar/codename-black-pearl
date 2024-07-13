using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProvider : MonoBehaviour, IDamageable
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void Damage()
    {
        health.Damage(1);
    }
}
