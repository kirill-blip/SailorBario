using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public event EventHandler PlayerEntered;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            PlayerEntered?.Invoke(this, null);
        }
    }
}
