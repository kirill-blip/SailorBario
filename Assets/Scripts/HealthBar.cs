using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void ChangeHealth(int health)
    {
        _healthImage.fillAmount = health / 100f;
        _healthText.text = health.ToString();
    }
}