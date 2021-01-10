using System;
using System.Collections;
using UnityEngine;

public class AggroChecker : MonoBehaviour
{
    [SerializeField] private Ratchet playerCharacter;
    [SerializeField] private ChainBlade chainBlade;
    private Collider _collider;

    public bool Aggroed { get; private set; }
    public event Action<bool> OnAggroChanged; 
    private void Awake()
    {
        playerCharacter.OnDie += RemoveAggro;
        chainBlade.OnHealthChanged += CheckForAggro;
        _collider = GetComponent<Collider>();
    }

   

    private void RemoveAggro()
    {
        StartCoroutine(RemoveAggroAndSetTriggerOffForSeconds(4));
    }

    private IEnumerator RemoveAggroAndSetTriggerOffForSeconds(int time)
    {
        yield return new WaitForSeconds(1f);
        Aggroed = false;
        OnAggroChanged?.Invoke(Aggroed);
        _collider.enabled = false;
        yield return new WaitForSeconds(time);
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Aggroed = true;
        OnAggroChanged?.Invoke(Aggroed);
    }
    
    private void CheckForAggro(float currentHealth, float maxHealth)
    {
        if (currentHealth < maxHealth - 1)
        {
            Aggroed = true;
            OnAggroChanged?.Invoke(Aggroed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,2f);
    }
}