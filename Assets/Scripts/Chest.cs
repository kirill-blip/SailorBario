using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Chest : InteractionBase
{
    [SerializeField] private int _coinCount = 10;
    [SerializeField] private float _timeScaling = .5f;
    [SerializeField] private AudioClip _clip;
    
    private bool _isOpen = false;
    
    private Animator _animator;
    private AudioSource _audioSource;
    
    public event EventHandler<int> ChestCollected;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        if (!_isOpen)
        {
            _animator.SetBool("IsOpening", true);
            _audioSource.PlayOneShot(_clip);
            _isOpen = !_isOpen;
        }
        else
        {
            _isOpen = !_isOpen;
            _animator.SetBool("IsOpening", false);
            ChestCollected?.Invoke(this, _coinCount);
            
            StartCoroutine(Destroy());
        }
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