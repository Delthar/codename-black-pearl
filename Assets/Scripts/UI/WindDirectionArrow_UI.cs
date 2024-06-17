using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindDirectionArrow_UI : MonoBehaviour
{
    [SerializeField] private RectTransform arrowRectTransform;
    [SerializeField] private Image arrowWindForceImage;

    [SerializeField, Range(1, 5)] private float fillDuration = 1;
    [SerializeField, Range(1, 5)] private float rotationDuration = 2;
    [SerializeField] private Ease fillEase = Ease.Unset;
    [SerializeField] private Ease rotationEase = Ease.Unset;

    private void OnEnable()
    {
        WindController.Instance.OnWindDirectionChanged += WindController_OnWindDirectionChanged;
        WindController.Instance.OnWindForceChanged += WindController_OnWindForceChanged;
    }

    private void OnDisable()
    {
        WindController.Instance.OnWindDirectionChanged -= WindController_OnWindDirectionChanged;
        WindController.Instance.OnWindForceChanged -= WindController_OnWindForceChanged;
    }

    private void WindController_OnWindForceChanged(object sender, WindController.OnWindForceChangedEventArgs e)
    {
        float fillAmount = arrowWindForceImage.fillAmount;
        DOTween.To(() => fillAmount, x => fillAmount = x, Mathf.InverseLerp(0, e.maxWindSpeed, e.newWindForce), fillDuration)
            .OnUpdate(() => {
                arrowWindForceImage.fillAmount = fillAmount;
            }).SetEase(fillEase);
        
    }

    private void WindController_OnWindDirectionChanged(object sender, WindController.OnWindDirectionChangedEventArgs e)
    {
        UpdateArrowRotation(e.newWindDirection);
    }

    private void UpdateArrowRotation(Vector2 direction)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, direction);
        float rot = arrowRectTransform.eulerAngles.z;
        DOTween.To(() => rot, x => rot = x, rotation.eulerAngles.z, rotationDuration)
            .OnUpdate(() => {
                arrowRectTransform.eulerAngles = new Vector3(0, 0, rot);
            }).SetEase(rotationEase);
        
    }
}
