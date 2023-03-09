using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
public class Crab : MonoBehaviour
{
    public float TimeToDestroy = 2;
    
    public int Damage = 5;

    public float TimeBetweenAttacks;
    public float SightRange;
    public float AttackRange;
    public float WalkPointRange;

    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;

    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private Animator _animator;
    private Health _health;

    private Vector3 _walkPoint;

    private bool _walkPointSet;
    private bool _alreadyAttacked;
    private bool _playerInSightRange;
    private bool _playerInAttackRange;

    private bool _canMove = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _health.CanDestroyWithoutAnimation = false;
        _health.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(object sender, int e)
    {
        if (e <= 0)
        {
            _animator.SetBool("IsDead", true);
            _agent.SetDestination(transform.position);
            ObjectDestroyer.DestroyInTime(this, .1f, TimeToDestroy);

            _canMove = false;
        }
    }

    private void Update()
    {
        if (!_canMove) return;

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

        if (Physics.Raycast(_walkPoint, -transform.up, 0.1f, WhatIsGround))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_playerTransform.position);
    }

    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position);

        if (!_alreadyAttacked)
        {
            RotateToPlayer();
            _animator.SetBool("IsAttacking", true);
            _playerTransform.GetComponent<Health>().TakeDamage(Damage);

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _animator.SetBool("IsAttacking", false);
        _alreadyAttacked = false;
    }

    private void RotateToPlayer()
    {
        var towards = _playerTransform.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(towards), _agent.angularSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, WalkPointRange);
    }
}