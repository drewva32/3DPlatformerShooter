using System;
using UnityEngine;

public class ChainBladeStateMachine : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private StateMachine _stateMachine;
    private ChainBlade _chainBlade;

    public Type CurrentStateType => _stateMachine.CurrentState.GetType();
    public event Action<IState> OnEntityStateChanged;

    private void Awake()
    {
        _chainBlade = GetComponent<ChainBlade>();
            
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnEntityStateChanged?.Invoke(state);
        
       var chase = new ChaseState(_chainBlade);
       var attack = new AttackState(_chainBlade);
       var idle = new IdleState(_chainBlade);
       var die = new DeadState(_chainBlade);
       var block = new BlockingState(_chainBlade);
       
       //death
        _stateMachine.AddAnyTransition(die, () => _chainBlade.CurrentHealth <= 0);
        //reset
        _stateMachine.AddAnyTransition(idle, () => _chainBlade.ratchet.CurrentHealth <= 0);
        //initialize fight if within range or chainblade takes dammage
        //Vector3.Distance(transform.position,playerTransform.position) < 20f && 
        _stateMachine.AddTransition(idle,chase, () => _chainBlade.AggroChecker.Aggroed);
        // _stateMachine.AddTransition(idle,chase, () => _chainBlade.CurrentHealth < _chainBlade.MaxHealth);
        
        
        _stateMachine.AddTransition(chase, block, () => _chainBlade.CurrentHealth > 0 && Vector3.Distance(transform.position,playerTransform.position) > 12f);
        _stateMachine.AddTransition(block,chase, () => Vector3.Distance(transform.position,playerTransform.position) < 10);
        _stateMachine.AddTransition(chase,attack, () => Vector3.Distance(transform.position,playerTransform.position) < 7f);
        _stateMachine.AddTransition(attack,chase, () => _chainBlade.DoneAttacking);
        
        _stateMachine.SetState(idle);
    }
    
    private void Update()
    {
        _stateMachine.Tick();
    }
}
