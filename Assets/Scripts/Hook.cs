using System;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public int Damage = 10;
    public bool CanMove = true;
    public bool IsMoving = false;

    public event EventHandler CollidedWithGround;
    public event EventHandler CollidedWithCrab;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsMoving) return;

        if (collision.gameObject.tag == "Ground")
        {
            CollidedWithGround?.Invoke(this, null);
            gameObject.LeanCancel();
            IsMoving = false;
        }

        if (collision.gameObject.TryGetComponent<Crab>(out Crab crab))
        {
            print("Crab damaged");
            crab.GetComponent<Health>().TakeDamage(Damage);
            CollidedWithCrab?.Invoke(this, null);
        }
    }
}