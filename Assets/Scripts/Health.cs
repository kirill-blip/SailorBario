using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool CanDestroyWithoutAnimation = true;
    
    [SerializeField] private int _currentHealth = 100;

    private const int _minHealth = 0;
    private const int _maxHealth = 100;

    public event EventHandler<int> HealthChanged;
    public event EventHandler Healed;
    public event EventHandler Killed;

    private void Start()
    {
        HealthChanged?.Invoke(this, _currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) 
            return;

        _currentHealth -= damage;
        
        HealthChanged?.Invoke(this, _currentHealth);

        if (_currentHealth <= _minHealth && CanDestroyWithoutAnimation)
        {
            Killed?.Invoke(this, null);
            Kill();
        }
    }

    public void Heal(int hitPoints)
    {
        if (hitPoints <= 0)
            return;

        _currentHealth += hitPoints;

        if (_currentHealth > _maxHealth) 
            _currentHealth = _maxHealth;

        HealthChanged?.Invoke(this, _currentHealth);
        Healed?.Invoke(this, null);
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void KillInTime(float time)
    {
        Invoke(nameof(Kill), time);
    }
}