using System;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<Dragon>().CoinsGoven += delegate(object sender, EventArgs args)
        {
            Destroy(this.gameObject);
        };
    }
}