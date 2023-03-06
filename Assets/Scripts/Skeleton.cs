using System;
using System.Collections;
using UnityEngine;

public class Skeleton : MonoBehaviour, IInteractable
{
    [SerializeField] private int _coinNeeded = 10;

    public event EventHandler PlayerEnteredOrExited;
    public event EventHandler CoinsGiven;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            PlayerEnteredOrExited?.Invoke(this, null);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            PlayerEnteredOrExited?.Invoke(this, null);
        }
    }
    
    private IEnumerator DestroyInTime()
    {
        var scale = new Vector3(.1f, .1f, .1f);
        gameObject.LeanScale(scale, 1);

        yield return new WaitUntil(() => gameObject.transform.localScale == scale);

        Destroy(gameObject);
    }

    public void Interact()
    {
        var playerController = FindObjectOfType<PlayerController>();
        
        if (playerController.Wallet.CanRemoveCoins(_coinNeeded))
        {
            StartCoroutine(DestroyInTime());
            CoinsGiven?.Invoke(this, null);
        }
    }
}