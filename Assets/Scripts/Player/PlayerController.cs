using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ShipController shipController;

    private CannonController cannonController;

    private void Awake()
    {
        shipController = GetComponent<ShipController>();
        cannonController = GetComponent<CannonController>();
    }

    private void Update()
    {
        // Movement
        Vector2 input = PlayerInput.Instance.GetMoveInput();
        shipController.SetForwardInput(input.y);
        shipController.SetAngularInput(input.x);

        // Cannons
        Vector3 mouseWorldPosition = Utilities.GetMouseWorldPosition();
        cannonController.Aim(mouseWorldPosition);
        if (PlayerInput.Instance.GetFireAction().IsPressed()) cannonController.Charge();
        else if (PlayerInput.Instance.GetFireAction().WasReleasedThisFrame()) cannonController.Fire(mouseWorldPosition, null);
    }
}
