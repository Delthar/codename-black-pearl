using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ShipController shipController;

    private void Awake()
    {
        shipController = GetComponent<ShipController>();
    }

    private void Update()
    {
        Vector2 input = PlayerInput.Instance.GetMoveInput();
        shipController.SetForwardInput(input.y);
        shipController.SetAngularInput(input.x);
    }
}
