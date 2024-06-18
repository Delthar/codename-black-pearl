using UnityEngine;
using UnityEngine.UI;

public class ShipSpeedIndicator_UI : MonoBehaviour
{
    [SerializeField] private ShipController shipController;
    [SerializeField] private Slider speedSlider;

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
    }
}
