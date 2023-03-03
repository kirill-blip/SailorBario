using System;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool CanMove = true;

    public event EventHandler CollidedWithGround;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            gameObject.LeanPause();
            CollidedWithGround?.Invoke(this, null);
        }
    }
}