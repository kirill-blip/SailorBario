﻿using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Crab : MonoBehaviour
{
    public int Damage = 5;

    public float TimeBetweenAttacks;
    public float SightRange;
    public float AttackRange;
    public float WalkPointRange;
    
    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;
    
    private NavMeshAgent _agent;
    private Transform _playerTransform;

    private Vector3 _walkPoint;
    
    private bool _walkPointSet;
    private bool alreadyAttacked;
    private bool _playerInSightRange;
    private bool _playerInAttackRange;

    private void Awake()
    {
        
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (!_playerInSightRange && !_playerInAttackRange) Patrolling();
        if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
        if (_playerInSightRange && _playerInAttackRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet)
            _agent.SetDestination(_walkPoint);

        var distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        var randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        var randomX = Random.Range(-WalkPointRange, WalkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, WhatIsGround))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_playerTransform.position);
    }

    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            _playerTransform.GetComponent<Health>().TakeDamage(Damage);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}