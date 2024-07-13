using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSpeedIndicator_UI : MonoBehaviour
{
    [SerializeField] private ShipController shipController;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private TMP_Text speedText;

    private void OnEnable()
    {
        shipController.OnVelocityChanged += ShipController_OnVelocityChanged;
    }

    private void OnDisable()
    {
        shipController.OnVelocityChanged -= ShipController_OnVelocityChanged;
        
    }

    private void ShipController_OnVelocityChanged(object sender, ShipController.OnVelocityChangedEventArgs e)
    {
        speedSlider.value = Mathf.InverseLerp(0, e.maxVelocity, Mathf.Abs(e.newVelocity));
        speedText.text = Mathf.Round(e.rigidbodySpeed).ToString();
    }
}
