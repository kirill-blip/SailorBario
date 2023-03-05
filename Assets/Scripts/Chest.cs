using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class Chest : MonoBehaviour, IOpenable, ICollectable
{
    [SerializeField] private int _coinCount = 10;
    [SerializeField] private float _timeScaling = .5f; 

    public  bool IsOpen { get; set; }
    
    private Animator _animator;

    public event EventHandler<int> ChestCollected;
    public event EventHandler PlayerEntered; 
    public event EventHandler PlayerExited; 

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            PlayerEntered?.Invoke(this,null);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            PlayerExited?.Invoke(this,null);
        }
    }

    public void Open()
    {
        IsOpen = true;
        _animator.SetBool("IsOpening", true);
    }

    public void Collect()
    {
        IsOpen = false;
        _animator.SetBool("IsOpening", false);
        ChestCollected?.Invoke(this, _coinCount);
        
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        var scaleIndex = .1f;
        var scale = new Vector3(scaleIndex, scaleIndex, scaleIndex);
        
        gameObject.LeanScale(scale, _timeScaling);
        
        yield return new WaitUntil(() => transform.localScale == scale);

        Destroy(this.gameObject);
    }
}