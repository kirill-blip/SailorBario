using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Wallet))]
public class PlayerController : MonoBehaviour
{
    private int _coinsCount;

    private Health _health;
    private PlayerMovement _playerMovement;
    private Wallet _wallet;

    public Health Health
    {
        get { return _health; }
    }

    public Wallet Wallet
    {
        get { return _wallet; }
    }

    private IInteractable _interactable;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _playerMovement = GetComponent<PlayerMovement>();
        _wallet = GetComponent<Wallet>();
    }

    private void Start()
    {
        var trees = FindObjectsOfType<Tree>();

        foreach (var item in trees)
        {
            item.FruitCollected += OnFruitCollected;
        }

        var chests = FindObjectsOfType<Chest>();

        foreach (var item in chests)
        {
            item.PlayerEnteredOrExited += OnPlayerEnteredOrExited;
            item.ChestCollected += OnChestCollected;
        }

        var gordon = FindObjectOfType<Skeleton>();
        gordon.PlayerEnteredOrExited += OnPlayerEnteredOrExited;

        FindObjectOfType<Tower>().PlayerEnteredOrExited += OnPlayerEnteredOrExited;
    }

    private void Update()
    {
        if (_interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            _interactable.Interact();
        }
    }

    private void OnPlayerEnteredOrExited(object sender, EventArgs e)
    {
        _interactable = _interactable == null ? sender as IInteractable : null;
    }

    private void OnChestCollected(object sender, int e)
    {
        _wallet.AddCoins(e);
    }

    private void OnFruitCollected(object sender, int e)
    {
        _health.Hill(e);
    }
}