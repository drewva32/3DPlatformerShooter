using System;
using UnityEngine;


public class DamageOnEnterTrigger : MonoBehaviour
{
    [SerializeField] private float attackDammage;
    // private void OnCollisionEnter(Collision other)
    // {
    //     var player = other.gameObject.GetComponent<Ratchet>();
    //     if(player)
    //         player.TakeDamage(attackDammage);
    // }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Ratchet>();
        if(player)
            player.TakeDamage(attackDammage);
    }
}
