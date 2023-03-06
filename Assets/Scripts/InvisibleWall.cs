using System;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<Skeleton>().CoinsGiven += delegate(object sender, EventArgs args)
        {
            Destroy(this.gameObject);
        };
    }
}