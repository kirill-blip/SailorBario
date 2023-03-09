using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Skeleton : InteractionBase
{
    [SerializeField] private int _coinNeeded = 10;
    [SerializeField] private Transform _positionWhereToGo;

    private bool _coinsIsGiven = false;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    public event EventHandler CoinsGiven;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        SecondTipText += $" {_coinNeeded} монет";
    }

    private void Update()
    {
        var distance = transform.position - _positionWhereToGo.position;

        if (distance.magnitude < 1f)
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetBool("IsWalking", false);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.back),
                Time.deltaTime * _navMeshAgent.angularSpeed);
        }
    }

    public override void Interact()
    {
        var playerController = FindObjectOfType<PlayerController>();

        if (_coinsIsGiven is false && playerController.Wallet.CanRemoveCoins(_coinNeeded))
        {
            _navMeshAgent.SetDestination(_positionWhereToGo.position);
            _animator.SetBool("IsWalking", true);

            TipText = string.Empty;
            OnTipTextChanged(this, TipText);

            CoinsGiven?.Invoke(this, null);

            _coinsIsGiven = true;
            CanInteract = false;
        }
        else if(_coinsIsGiven is false)
        {
            OnTipTextChanged(this, SecondTipText);
        }
    }
}