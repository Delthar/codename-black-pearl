using UnityEngine;
using UnityEngine.InputSystem;

public static class Utilities
{
    /// <summary>
    /// Gets the mouse position in World Space
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMouseWorldPosition()
    {
        Vector2 cursorLocalPosition = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(new Vector3(cursorLocalPosition.x, cursorLocalPosition.y, Camera.main.nearClipPlane));
    }
}
