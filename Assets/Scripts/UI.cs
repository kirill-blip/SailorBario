using System;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;

    private void Start()
    {
        FindObjectOfType<PlayerMovement>().HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(object sender, int e)
    {
        print("Hello, World");
        _healthText.text = e.ToString();
    }
}