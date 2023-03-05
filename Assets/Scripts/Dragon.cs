using System;
using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [SerializeField] private int _coinNeeded = 10;

    private bool _playerIsHere = false;

    public event EventHandler PlayerEntered;
    public event EventHandler PlayerExited;
    public event EventHandler CoinsGoven;

    private void Update()
    {
        if (_playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            var player = FindObjectOfType<PlayerController>();
            
            if (player.GetCoins() == _coinNeeded)
            {
                CoinsGoven?.Invoke(this, null);
                player.RemoveCoins(_coinNeeded);
                StartCoroutine(DestroyInTime());
            }
        }
    }

    private IEnumerator DestroyInTime()
    {
        var scale = new Vector3(.1f, .1f, .1f);
        gameObject.LeanScale(scale, 2);

        yield return new WaitUntil(() => gameObject.transform.localScale == scale);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            _playerIsHere = true;
            PlayerEntered?.Invoke(this, null);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            _playerIsHere = false;
            PlayerExited?.Invoke(this, null);
        }
    }
}