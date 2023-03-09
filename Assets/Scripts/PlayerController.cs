using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Wallet))]
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioClip _healUpSound;
    [SerializeField] private Transform _startPosition;

    private int _coinsCount;

    #region Components

    private Health _health;
    private InputHandler _inputHandler;
    private Wallet _wallet;
    private AudioManager _audioManager;
    private GrapplingGun _grapplingGun;

    #endregion
    #region Properties

    public Health Health
    {
        get { return _health; }
    }
    public Wallet Wallet
    {
        get { return _wallet; }
    }
    public InputHandler InputHandler
    {
        get { return _inputHandler; }
    }

    #endregion
    
    private IInteractionAware _interactionAware;
    private Key _key;

    public event EventHandler PlayerDead;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _wallet = GetComponent<Wallet>();
        _inputHandler = GetComponent<InputHandler>();
        _audioManager = FindObjectOfType<AudioManager>();
        _grapplingGun = FindObjectOfType<GrapplingGun>();
    }

    private void Start()
    {
        LeanTween.init(200000);
        
        var trees = FindObjectsOfType<Tree>().ToList();
        trees.ForEach(tree => { tree.FruitCollected += OnFruitCollected; });

        var chests = FindObjectsOfType<Chest>();
        
        foreach (var chest in chests)
        {
            chest.PlayerEnteredOrExited += OnPlayerEnteredOrExited;
            chest.ChestCollected += OnChestCollected;
        }

        var gordonFreeman = FindObjectOfType<Skeleton>();
        gordonFreeman.PlayerEnteredOrExited += OnPlayerEnteredOrExited;

        var tower = FindObjectOfType<Tower>();
        tower.PlayerEnteredOrExited += OnPlayerEnteredOrExited;
        tower.PlayerPressed += OnPlayerPressed;
        FindObjectOfType<Key>().KeyCollected += OnKeyCollected;
        FindObjectOfType<PlayerDetector>().PlayerEntered += OnPlayerEntered;

        _health.Healed += (sender, args) => { _audioManager.PlaySound(_healUpSound); };
        _health.Killed += (sender, args) =>
        {
            Camera.main.transform.parent = null;
            Camera.main.GetComponent<MouseLook>().enabled = false;
            PlayerDead?.Invoke(this, null);
            Destroy(_grapplingGun.gameObject);
        };
    }

    private void OnPlayerPressed(object sender, EventArgs e)
    {
        _inputHandler.DisableMovingAndShooting();
    }

    private void Update()
    {
        if (_interactionAware != null && Input.GetKeyDown(KeyCode.E))
        {
            _interactionAware.Interact();
        }
    }

    private void OnKeyCollected(object sender, Key key)
    {
        _key = key;
    }

    private void OnPlayerEntered(object sender, EventArgs args)
    {
        transform.position = _startPosition.position;
    }

    private void OnPlayerEnteredOrExited(object sender, EventArgs args)
    {
        _interactionAware = _interactionAware == null ? sender as IInteractionAware : null;
    }

    private void OnChestCollected(object sender, int coins)
    {
        _wallet.AddCoins(coins);
    }

    private void OnFruitCollected(object sender, int healPoints)
    {
        _health.Heal(healPoints);
    }

    public bool HasKey()
    {
        return _key is not null;
    }
}