using System;
using PathologicalGames;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int attackDamage;
    private Collider _collider;
    private Rigidbody _rb;
    private bool _despawned;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _collider.isTrigger = true;
    }

    public void OnSpawned()
    {
        _despawned = false;
        _rb.AddForce(transform.forward * moveSpeed);
        if (!_despawned)
        {
            _despawned = true;
            PoolManager.Pools["EnemyProj"].Despawn(this.transform,4f);
        }
    }

    public void OnDespawned()
    {
        _rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("projectile triggered");
        Debug.Log(other.gameObject.name);
        var player = other.GetComponent<Ratchet>();
        if(player)
            player.TakeDamage(attackDamage);
        
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     var player = other.collider.GetComponent<Ratchet>();
    //     if(player)
    //         player.TakeDamage(attackDamage);
    // }
}