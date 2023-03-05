using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _currentHealth = 100;

    private const int _minHealth = 0;
    private const int _maxHealth = 100;

    public event EventHandler<int> HealthChanged;

    private void Start()
    {
        HealthChanged?.Invoke(this, _currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        _currentHealth -= damage;

        HealthChanged?.Invoke(this, _currentHealth);

        if (_currentHealth <= _minHealth) Kill();
    }

    public void Hill(int hillPoint)
    {
        if (hillPoint <= 0) return;

        _currentHealth += hillPoint;

        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;

        HealthChanged?.Invoke(this, _currentHealth);
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}