using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer hullSpriteRenderer;
    [SerializeField] private SpriteRenderer sailSpriteRenderer;
    [SerializeField] private List<ShipPart> shipParts;

    [Serializable]
    public class ShipPart
    {
        public Texture2D hull;
        public Texture2D sail;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
    }

    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void Health_OnHealthChanged(object sender, Health.OnHealthChangedEventArgs e)
    {
        
    }
}
