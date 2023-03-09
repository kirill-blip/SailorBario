using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10;

    public event EventHandler<Key> KeyCollected;

    private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            KeyCollected?.Invoke(this, this);
            ObjectDestroyer.DestroyInTime(this, .1f, 1f);
        }
    }
}