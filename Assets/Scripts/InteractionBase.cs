using System;
using UnityEngine;

public abstract class InteractionBase : MonoBehaviour, IInteractionAware
{
    public event EventHandler PlayerEnteredOrExited;

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
    
    protected void OnPlayerEnteredOrExited()
    {
        PlayerEnteredOrExited?.Invoke(this, null);
    }
    
    public abstract void Interact();
}