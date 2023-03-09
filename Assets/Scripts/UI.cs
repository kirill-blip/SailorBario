using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _darkScreen;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private float _time = .01f;
    
    #region Texts

    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;

    #endregion
    #region Buttons

    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _restartButton;

    #endregion

    private PlayerController _playerController;
    private AudioManager _audioManager;

    public event EventHandler ReturnButtonClicked;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();

        _playerController = FindObjectOfType<PlayerController>();
        _playerController.Health.HealthChanged += OnHealthChanged;
        _playerController.Wallet.CoinsCountChanged += OnCoinsCountChanged;
        _playerController.InputHandler.EscapePressed += OnEscapePressed;
        _playerController.PlayerDead += delegate(object sender, EventArgs args)
        {
            ActivateCursor();
            OnEscapePressed(sender, args);
        };

        var interactionObjects = FindObjectsOfType<InteractionBase>();

        foreach (var interactionObject in interactionObjects)
        {
            interactionObject.TipTextChanged += OnPlayerEnteredOrExited;
        }

        var tower = FindObjectOfType<Tower>();
        tower.PlayerPressed += OnPlayerPressed;

        _returnButton.onClick.AddListener(OnReturnButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        _audioManager.PlayButtonSound();
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    private void OnReturnButtonClicked()
    {
        _audioManager.PlayButtonSound();
        ReturnButtonClicked?.Invoke(this, null);
        _pausePanel.SetActive(false);
    }

    private void OnExitButtonClicked()
    {
        _audioManager.PlayButtonSound();
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    private void OnEscapePressed(object sender, EventArgs e)
    {
        _pausePanel.SetActive(!_pausePanel.activeInHierarchy);
    }

    private void OnPlayerPressed(object sender, EventArgs e)
    {
        StartCoroutine(ChangeColor());
    }

    private void OnCoinsCountChanged(object sender, int e)
    {
        _coinsCountText.text = e.ToString();
    }

    private void OnPlayerEnteredOrExited(object sender, string e)
    {
        _tipText.text = e;
    }

    private void OnHealthChanged(object sender, int health)
    {
        _healthBar.ChangeHealth(health);
    }

    private void ActivateCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator ChangeColor()
    {
        _darkScreen.gameObject.SetActive(true);

        _audioManager.PlayEndSound();

        while (_darkScreen.color != Color.black)
        {
            Color color = Vector4.MoveTowards(_darkScreen.color, Color.black, Time.deltaTime * 5);
            _darkScreen.color = color;
            yield return new WaitForSeconds(_time);
        }
        
        ActivateCursor();
        OnEscapePressed(null, null);
        Time.timeScale = 0;
    }
}