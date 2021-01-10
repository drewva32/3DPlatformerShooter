using System;
using System.Collections;
using ECM.Common;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour,ITargetable
{
    [Header("Player Weapon Targeting")]
    [SerializeField] private Transform targetPoint;
    [Header("EnemyStats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Collider _collider;
    private float _currentHealth;
    private float _distanceToPlayer;
    private bool _attacking;
    private Coroutine _chasing;
    private Coroutine _idleCheck;
    private Vector3 _startPosition;
    private bool _dead;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Hit = Animator.StringToHash("Hit");


    public Transform TargetPoint => targetPoint;

    public bool Dead => _dead;

    public event Action Die;
    public bool IsAggroed { get; set; }
    
    
    private void Awake()
    {
        _startPosition = transform.position;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _currentHealth = maxHealth;
    }

    public void GetShot(float damage, Vector3 positionOfDamager)
    {
        var directionOfHit = (transform.position.onlyXZ() - positionOfDamager.onlyXZ()).normalized;
        
        _animator.SetTrigger(Hit);
        _navMeshAgent.isStopped = true;
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(directionOfHit * 400);
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _collider.enabled = false;
            _dead = true;
            Die?.Invoke();
            Invoke(nameof(Deactivate),1.5f);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        Debug.Log("dead");
    }

    public void ChaseTarget(Transform target)
    {
        _chasing = StartCoroutine(Chase(target));
    }

    public void StopChasingTarget()
    {
        StopCoroutine(_chasing);
    }

    private IEnumerator Chase(Transform target)
    {
        if (_idleCheck != null)
        {
            StopCoroutine(_idleCheck);
            _idleCheck = null;
        }
        _animator.SetBool(Walking, true);
        while (IsAggroed && !_dead)
        {
            _navMeshAgent.SetDestination(target.position);
            if (IsInAttackRange(target) && !_attacking) 
                Attack();
            yield return new WaitForSeconds(.1f);
        }
    }

    private bool IsInAttackRange(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < attackRange;
    }

    private void Attack()
    {
        StartCoroutine(PlayAttack());
    }

    private IEnumerator PlayAttack()
    {
        _animator.SetTrigger(Attack1);
        _attacking = true;
        yield return new WaitForSeconds(3f);
        _attacking = false;
    }

    public void DamagePlayer()
    {
        var hits = Physics.OverlapSphere(TargetPoint.position, 2f, playerLayer);
        if (hits.Length < 1)
            return;
        var ratchet = hits[0].GetComponent<Ratchet>();
        if (ratchet)
        {
            ratchet.TakeDamage(attackDamage);
        }
    }


    public void ReturnToSpawnPosition( )
    {
        _navMeshAgent.SetDestination(_startPosition);
        _idleCheck = StartCoroutine(CheckIfAtSpawnPoint());
    }

    private IEnumerator CheckIfAtSpawnPoint()
    {
        while (!IsAggroed)
        {
            var distanceToSpawnPoint = Vector3.Distance(transform.position, _startPosition);
            if (distanceToSpawnPoint < 3f) 
                _animator.SetBool("Walking", false);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnDisable()
    {
        _dead = true;
    }
}