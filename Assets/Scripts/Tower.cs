using System;
using UnityEngine;

public class Tower : MonoBehaviour, IInteractable
{
    public event EventHandler PlayerEnteredOrExited;
    public event EventHandler PlayerPressed;

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

    public void Interact()
    {
        PlayerPressed?.Invoke(this, null);
        PlayerEnteredOrExited?.Invoke(this, null);
    }
}