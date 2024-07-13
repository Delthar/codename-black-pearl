using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipColorizer : MonoBehaviour
{
    [SerializeField] private Color shipColor;
    [SerializeField] private SpriteRenderer sailSpriteRenderer;
    [SerializeField] private SpriteRenderer poleFlagSpriteRenderer;
    [SerializeField] private SpriteRenderer frontFlagSpriteRenderer;

    private void Start()
    {
        SetColor(shipColor);
    }

    public void SetColor(Color color)
    {
        color.a = 1;
        sailSpriteRenderer.color = color;
        poleFlagSpriteRenderer.color = color;
        frontFlagSpriteRenderer.color = color;
    }
}
