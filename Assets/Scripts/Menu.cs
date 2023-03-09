using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _startGame;
    [SerializeField] private Button _exitGame;

    private AudioManager _audioManager;
    private const float _time = .1f;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        _startGame.onClick.AddListener(delegate { StartCoroutine(StartGame()); });
        _exitGame.onClick.AddListener(delegate { StartCoroutine(ExitGame()); });
    }

    private IEnumerator ExitGame()
    {
        _audioManager.PlayButtonSound();

        yield return new WaitForSeconds(_time);

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    private IEnumerator StartGame()
    {
        _audioManager.PlayButtonSound();

        yield return new WaitForSeconds(_time);

        SceneManager.LoadScene("Tutorial");
    }
}