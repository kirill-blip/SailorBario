using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private int _coinsCount;

    private Health _health;
    private PlayerMovement _playerMovement;

    public event EventHandler<int> CoinsCountChanged;

    public Health Health
    {
        get { return _health; }
    }

    private bool _isNearChest = false;
    private Chest _chest;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _playerMovement = GetComponent<PlayerMovement>();
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
            item.PlayerEntered += OnPlayerEntered;
            item.PlayerExited += OnPlayerExited;
            item.ChestCollected += OnChestCollected;
        }
    }

    private void Update()
    {
        if (_isNearChest && Input.GetKeyDown(KeyCode.E))
        {
            if (!_chest.IsOpen)
            {
                _chest.Open();
            }
            else
            {
                _chest.Collect();
            }
        }
    }

    private void OnPlayerExited(object sender, EventArgs e)
    {
        _chest = null;
        _isNearChest = false;
    }

    private void OnPlayerEntered(object sender, EventArgs e)
    {
        _isNearChest = true;
        _chest = sender as Chest;
    }

    private void OnChestCollected(object sender, int e)
    {
        _coinsCount += e;
        CoinsCountChanged?.Invoke(this, _coinsCount);
    }

    private void OnFruitCollected(object sender, int e)
    {
        _health.Hill(e);
    }

    public int GetCoins()
    {
        return _coinsCount;
    }

    public void RemoveCoins(int coins)
    {
        _coinsCount -= coins;
        CoinsCountChanged?.Invoke(this, _coinsCount);
    }
}