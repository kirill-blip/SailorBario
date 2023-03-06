using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Wallet))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioClip _healUpSound;

    private int _coinsCount;

    private Health _health;
    private PlayerMovement _playerMovement;
    private Wallet _wallet;
    private AudioManager _audioManager;

    public Health Health
    {
        get { return _health; }
    }

    public Wallet Wallet
    {
        get { return _wallet; }
    }

    private IInteractionAware _interactionAware;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _playerMovement = GetComponent<PlayerMovement>();
        _wallet = GetComponent<Wallet>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
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

        FindObjectOfType<Tower>().PlayerEnteredOrExited += OnPlayerEnteredOrExited;

        _health.Healed += (sender, args) => { _audioManager.PlaySound(_healUpSound); };
    }

    private void Update()
    {
        if (_interactionAware != null && Input.GetKeyDown(KeyCode.E))
        {
            _interactionAware.Interact();
        }
    }

    private void OnPlayerEnteredOrExited(object sender, EventArgs e)
    {
        // _interactionAware ??= sender as IInteractionAware;

        _interactionAware = _interactionAware == null ? sender as IInteractionAware : null;
    }

    private void OnChestCollected(object sender, int e)
    {
        _wallet.AddCoins(e);
    }

    private void OnFruitCollected(object sender, int e)
    {
        _health.Heal(e);
    }
}