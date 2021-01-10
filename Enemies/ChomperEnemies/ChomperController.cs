using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperController : MonoBehaviour
{
    private Enemy[] _childEnemies;

    private void Awake()
    {
        _childEnemies = GetComponentsInChildren<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var enemy in _childEnemies)
            {
                if (enemy.Dead) continue;
                enemy.IsAggroed = true;
                enemy.ChaseTarget(other.transform);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var enemy in _childEnemies)
            {
                if (enemy.Dead) continue;
                enemy.IsAggroed = false;
                enemy.ReturnToSpawnPosition();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,10f);
        
    }
}
