using UnityEngine;

public class AttackState : IState
{
    private readonly ChainBlade _chainBlade;

    private Coroutine _attack;
    private string lastAttackClip;
    public AttackState(ChainBlade chainBlade)
    {
        _chainBlade = chainBlade;
    }
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        _chainBlade.ShieldCollider.enabled = false;
        var distanceToPlayer = Vector3.Distance(_chainBlade.transform.position, _chainBlade.PlayerTransform.position);
        if (lastAttackClip == "Attack" && distanceToPlayer < 6)
        {
            //possibly add slash 3 here as another short range quick attack(slightly more range than kick)
            _attack = _chainBlade.StartCoroutine(_chainBlade.StartKick("Kick"));
            lastAttackClip = "Kick";
        }
        else
        {
            _attack = _chainBlade.StartCoroutine(_chainBlade.StartAttack("Attack"));
            lastAttackClip = "Attack";
        }
    }

    public void OnExit()
    {
        _chainBlade.ShieldCollider.enabled = true;
        _chainBlade.StopCoroutine(_attack);
        _chainBlade.DoneAttacking = false;
    }
}