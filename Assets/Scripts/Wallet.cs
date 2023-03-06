using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _coinsCount;

    public event EventHandler<int> CoinsCountChanged;

    public bool CanRemoveCoins(int coins)
    {
        if (coins > _coinsCount || coins <= 0)
            return false;

        _coinsCount -= coins;
        CoinsCountChanged?.Invoke(this, _coinsCount);

        return true;
    }

    public void AddCoins(int coins)
    {
        if (coins <= 0) return;

        _coinsCount += coins;
        CoinsCountChanged?.Invoke(this, _coinsCount);
    }
}