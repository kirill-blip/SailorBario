using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [SerializeField] private TextMeshProUGUI _dragonWordsText;
    [SerializeField] private Image _darkScreen;

    [SerializeField] private float _time = .01f;

    private PlayerController _playerController;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerController.Health.HealthChanged += OnHealthChanged;
        _playerController.Wallet.CoinsCountChanged += OnCoinsCountChanged;

        var chests = FindObjectsOfType<Chest>();

        foreach (var item in chests)
        {
            item.PlayerEnteredOrExited += OnPlayerEnteredOrExited;
        }

        var skeleton = FindObjectOfType<Skeleton>();
        skeleton.PlayerEnteredOrExited += SkeletonOnPlayerEnteredOrExited;

        var tower = FindObjectOfType<Tower>();
        tower.PlayerEnteredOrExited += OnPlayerEnteredOrExited;
        tower.PlayerPressed += OnPlayerPressed;
    }

    private void OnPlayerPressed(object sender, EventArgs e)
    {
        StartCoroutine(ChangeColor());
    }

    private void SkeletonOnPlayerEnteredOrExited(object sender, EventArgs e)
    {
        _dragonWordsText.gameObject.SetActive(!_dragonWordsText.gameObject.activeInHierarchy);
    }

    private void OnCoinsCountChanged(object sender, int e)
    {
        _coinsCountText.text = e.ToString();
    }

    private void OnPlayerEnteredOrExited(object sender, EventArgs e)
    {
        _tipText.gameObject.SetActive(!_tipText.gameObject.activeInHierarchy);
    }

    private void OnHealthChanged(object sender, int e)
    {
        _healthText.text = e.ToString();
    }

    private IEnumerator ChangeColor()
    {
        _darkScreen.gameObject.SetActive(true);

        while (_darkScreen.color != Color.black)
        {
            Color color = Vector4.MoveTowards(_darkScreen.color, Color.black, Time.deltaTime * 5);
            _darkScreen.color = color;
            yield return new WaitForSeconds(_time);
        }
    }
}