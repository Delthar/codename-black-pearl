using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProvider : MonoBehaviour, IDamageable
{
    private Health health;
    private Rigidbody2D rb;
    private ShipController shipController;
    private Collider2D col;

    private void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        shipController = GetComponent<ShipController>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        health.OnDied += Health_OnDied;
    }

    private void OnDisable()
    {
        health.OnDied -= Health_OnDied;
    }
    
    private void Health_OnDied(object sender, EventArgs e)
    {
        shipController.Disable();
        rb.angularDrag = 1;
        rb.drag = 1;
        col.enabled = false;
    }

    public void Damage()
    {
        health.Damage(1);
    }
}
