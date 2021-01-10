using System;
using System.Collections;
using System.Collections.Generic;
using ECM.Common;
using PathologicalGames;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactParticles;
    [Header("Stats")] [SerializeField] private float speed = 800f;
    [SerializeField] private float damage = 1;
    [Header("Aoe")] [SerializeField] private bool aoe;
    [SerializeField] private float aoeSize;
    [SerializeField] private float aoeDamage;
    [SerializeField] private float maxMoveSpeed;

    private Rigidbody _rb;
    private bool _despawned;
    private ITargetable _enemyToFollow;
    private bool _recievedTargetThisFrame;

    public bool FollowTarget { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnSpawned()
    {
        _rb.AddForce(transform.forward * speed);
        _despawned = false;
        Debug.Log("Spawned");
    }

    public void OnDespawned()
    {
        _despawned = true;
        _rb.velocity = Vector3.zero;
        FollowTarget = false;
        _enemyToFollow = null;
    }

    private void Update()
    {
        if (!FollowTarget)
            return;
        var enemyDirection = (_enemyToFollow.TargetPoint.position - transform.position).normalized;

        if (_recievedTargetThisFrame)
        {
            _rb.velocity = Vector3.zero;
            _rb.AddForce(enemyDirection * speed);
            _recievedTargetThisFrame = false;
        }
        
        if (Vector3.Dot(_rb.velocity.normalized, enemyDirection) < 0)
        {
            FollowTarget = false;//turns off heat seeking target if projectile pass the target to avoid projectiles turning 180 degrees.
        }
        var desiredVelocity = enemyDirection * maxMoveSpeed;
        var steering = desiredVelocity - _rb.velocity;
        _rb.AddForce(steering);
    }

    public void SetTargetEnemy(ITargetable enemy)
    {
        _enemyToFollow = enemy;
        _enemyToFollow.Die += EnemyToFollowOnDie;
        FollowTarget = true;
        _recievedTargetThisFrame = true;

    }

    private void EnemyToFollowOnDie()
    {
        FollowTarget = false;
    }

    private void OnDisable()
    {
        if (FollowTarget)
            _enemyToFollow.Die -= EnemyToFollowOnDie;
    }


    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.collider.GetComponent<IGetShot>();
        var impactNormal = other.contacts[0].normal;
        PoolManager.Pools["WeaponParticles"]
            .Spawn(impactParticles.transform, transform.position, Quaternion.Euler(impactNormal));
        if (enemy != null)
        {
            enemy.GetShot(damage,transform.position);
        }

        if (!_despawned)
            PoolManager.Pools["Projectiles"].Despawn(transform);
    }
}