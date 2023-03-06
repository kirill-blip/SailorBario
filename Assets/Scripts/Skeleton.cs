using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Skeleton : InteractionBase
{
    [SerializeField] private int _coinNeeded = 10;
    [SerializeField] private Transform _positionWhereToGo;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public event EventHandler CoinsGiven;

    private IEnumerator DestroyInTime()
    {
        var scale = new Vector3(.1f, .1f, .1f);
        gameObject.LeanScale(scale, 1);

        yield return new WaitUntil(() => gameObject.transform.localScale == scale);

        Destroy(gameObject);
    }

    private void Update()
    {
        var distance = transform.position - _positionWhereToGo.position;

        if (distance.magnitude < 1f)
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetBool("IsWalking", false);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.right),
                Time.deltaTime * _navMeshAgent.angularSpeed);
        }
    }

    public override void Interact()
    {
        var playerController = FindObjectOfType<PlayerController>();

        if (playerController.Wallet.CanRemoveCoins(_coinNeeded))
        {
            // StartCoroutine(DestroyInTime());
            _navMeshAgent.SetDestination(_positionWhereToGo.position);
            _animator.SetBool("IsWalking", true);

            CoinsGiven?.Invoke(this, null);
        }
    }
}