using System;
using UnityEngine;

public abstract class InteractionBase : MonoBehaviour, IInteractionAware
{
    public string TipText;
    public string SecondTipText;

    public bool CanInteract { get; set; } = true;

    public event EventHandler PlayerEnteredOrExited;
    public event EventHandler<string> TipTextChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (CanInteract && other.TryGetComponent(out PlayerController _))
        {
            PlayerEnteredOrExited?.Invoke(this, null);
            TipTextChanged?.Invoke(this, TipText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            PlayerEnteredOrExited?.Invoke(null, null);
            TipTextChanged?.Invoke(this, string.Empty);
        }
    }

    protected void OnTipTextChanged(object sender, string tipText)
    {
        TipTextChanged?.Invoke(sender, tipText);
    }

    public abstract void Interact();
}