using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler OnDied;
    public class OnHealthChangedEventArgs : EventArgs
    {
        public bool decreased;
        public int oldHealth;
        public int newHealth;
    }

    [SerializeField, Min(1)] private int maxHealth = 3;
    private int _currentHealth;

    private void Awake() => _currentHealth = maxHealth;

    public void Damage(int amount)
    {
        if (_currentHealth == 0) return;

        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Clamp(_currentHealth - Mathf.Abs(amount), 0, maxHealth);
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { decreased = oldHealth > _currentHealth, oldHealth = oldHealth, newHealth = _currentHealth });
        if (_currentHealth == 0)
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Heal(int amount)
    {
        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Clamp(_currentHealth + Mathf.Abs(amount), 0, maxHealth);
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { decreased = oldHealth > _currentHealth, oldHealth = oldHealth, newHealth = _currentHealth });
    }

    public void SetHealth(int amount)
    {
        if (_currentHealth == amount) return;

        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Clamp(Mathf.Abs(amount), 0, maxHealth);
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { decreased = oldHealth > _currentHealth, oldHealth = oldHealth, newHealth = _currentHealth });
        if (_currentHealth == 0)
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void SetMaxHealth(int amount) => maxHealth = amount;
    public int GetMaxHealth() => maxHealth;
    public int GetHealth() => _currentHealth;
    public bool IsDead() => _currentHealth == 0;
    public int GetHealthNormalized() => _currentHealth / maxHealth;
}
