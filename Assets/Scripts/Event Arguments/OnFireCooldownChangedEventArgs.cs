using System;

public class OnFireCooldownChangedEventArgs : EventArgs
{
    public float fireCooldown;
    public float currentFireCooldown;

    public OnFireCooldownChangedEventArgs(float fireCooldown, float currentFireCooldown)
    {
        this.fireCooldown = fireCooldown;
        this.currentFireCooldown = currentFireCooldown;
    }
}