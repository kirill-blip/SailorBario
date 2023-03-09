using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Chest : InteractionBase
{
    [SerializeField] private int _coinCount = 10;
    [SerializeField] private float _timeScaling = .5f;
    [SerializeField] private AudioClip _clip;

    private bool _isOpen;

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
            OnTipTextChanged(this, SecondTipText);
            _isOpen = !_isOpen;
            
            _animator.SetBool("IsOpening", true);
            _audioSource.PlayOneShot(_clip);
        }
        else
        {
            OnTipTextChanged(this, string.Empty);
            _isOpen = !_isOpen;
            
            _animator.SetBool("IsOpening", false);
            ChestCollected?.Invoke(this, _coinCount);
            ObjectDestroyer.DestroyInTime(this, .1f, _timeScaling);
        }
    }
}