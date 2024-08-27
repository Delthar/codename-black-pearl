using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CannonFireIndicator_UI : MonoBehaviour
{
    [Tooltip("The reference to the CannonController of the player")]
    [SerializeField] private CannonController cannonController;
    [Tooltip("The reference to the Foreground")]
    [SerializeField] private Image foregroundImage;
    [Tooltip("The reference to the Image")]
    [SerializeField] private RectTransform indicatorRectTransform;
    [Tooltip("The reference to the Cooldown Text")]
    [SerializeField] private TMP_Text cooldownText;
    [Tooltip("The type of closing movement of the fire cooldown end")]
    [SerializeField] private Ease fireCooldownEndEase = Ease.Unset;
    [Tooltip("The duration of the fire cooldown end movement")]
    [Range(0.1f, 1)]
    [SerializeField] private float fireCooldownEndDuration = 0.25f;
    [Tooltip("The intensity of the fire cooldown end movement")]
    [Range(0.1f, 1)]
    [SerializeField] private float fireCooldownEndIntensity = 0.25f;
    [Tooltip("The vibration of the fire cooldown end movement")]
    [Range(1, 4)]
    [SerializeField] private int fireCooldownEndVibrato = 2;
    [Tooltip("The scale multiplier of the image during fire cooldown")]
    [Range(0.1f, 1)]
    [SerializeField] private float imageScaleMultiplier = 0.5f;

    private void OnEnable()
    {
        cannonController.OnFire += CannonController_OnFire;
        cannonController.OnFireCooldownChanged += CannonController_OnFireCooldownChanged;
        cannonController.OnFireCooldownEnd += CannonController_OnFireCooldownEnd;
    }

    private void OnDisable()
    {
        cannonController.OnFire -= CannonController_OnFire;
        cannonController.OnFireCooldownChanged -= CannonController_OnFireCooldownChanged;
        cannonController.OnFireCooldownEnd -= CannonController_OnFireCooldownEnd;
    }

    private void CannonController_OnFire(object sender, EventArgs e)
    {
        foregroundImage.fillAmount = 0;
    }

    private void CannonController_OnFireCooldownChanged(object sender, CannonController.OnFireCooldownChangedEventArgs e)
    {
        float normalized = Mathf.InverseLerp(e.fireCooldown, 0, e.currentFireCooldown);

        cooldownText.text = (e.currentFireCooldown > 0) ? Mathf.Abs(e.currentFireCooldown).ToString("0.0") : string.Empty;
        indicatorRectTransform.localScale = Vector3.one * (1 - imageScaleMultiplier + normalized * imageScaleMultiplier);
        foregroundImage.fillAmount = normalized;
    }

    private void CannonController_OnFireCooldownEnd(object sender, EventArgs e)
    {
        indicatorRectTransform.DOPunchScale(Vector3.one * fireCooldownEndIntensity, fireCooldownEndDuration, fireCooldownEndVibrato).SetEase(fireCooldownEndEase);
    }
}
