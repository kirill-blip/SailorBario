using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Transform _player;

    public event EventHandler PlayerIsHere;
    public event EventHandler PlayerPressed;
    
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (Vector3.Distance(_player.position, transform.position) < 10)
        {
            PlayerIsHere?.Invoke(this, null);

            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerPressed?.Invoke(this, null);
            }
        }
    }
}