using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer hullSpriteRenderer;
    [SerializeField] private SpriteRenderer sailSpriteRenderer;
    [SerializeField] private SpriteRenderer frontSailSpriteRenderer;
    [SerializeField] private SpriteRenderer poleFlagSpriteRenderer;
    [SerializeField] private SpriteRenderer poleNestSpriteRenderer;
    [SerializeField] private List<SpriteRenderer> cannonSpriteRenderers;
    [SerializeField] private List<ShipPart> shipParts;

    [Serializable]
    public class ShipPart
    {
        public Sprite hull;
        public Sprite sail;
        public Sprite frontSail;
        public Sprite pole;
        public Sprite poleNest;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
        health.OnDied -= Health_OnDied;
    }

    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChanged;
        health.OnDied += Health_OnDied;
    }

    private void Health_OnDied(object sender, EventArgs e)
    {
        hullSpriteRenderer.DOFade(0, 5);
        sailSpriteRenderer.DOFade(0, 5);
        frontSailSpriteRenderer.DOFade(0, 5);
        poleFlagSpriteRenderer.DOFade(0, 5);
        poleNestSpriteRenderer.DOFade(0, 5);
        foreach(SpriteRenderer renderer in cannonSpriteRenderers)
        {
            renderer.DOFade(0, 5);
        }
        Destroy(gameObject, 5);
    }

    private void Health_OnHealthChanged(object sender, Health.OnHealthChangedEventArgs e)
    {
        float percent = (float)e.newHealth / health.GetMaxHealth();
        float lerp = Mathf.Lerp(0, shipParts.Count - 1, percent);
        int shipPartIndex = Mathf.CeilToInt(lerp);
        hullSpriteRenderer.sprite = shipParts[shipPartIndex].hull;
        sailSpriteRenderer.sprite = shipParts[shipPartIndex].sail;
        frontSailSpriteRenderer.sprite = shipParts[shipPartIndex].frontSail;
        poleFlagSpriteRenderer.sprite = shipParts[shipPartIndex].pole;
        poleNestSpriteRenderer.sprite = shipParts[shipPartIndex].poleNest;
    }
}
