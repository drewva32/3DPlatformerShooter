using UnityEngine;

public class ChaseState : IState
{
    private ChainBlade _chainBlade;
    //constructor with whatever classes are needed
    // private UIManager _uiManager;
    //
    // public S_ActiveGame(UIManager uiManager)
    // {
    //     _uiManager = uiManager;
    // }
    private Coroutine _chase;
    private bool _initialized;

    public ChaseState(ChainBlade chainBlade)
    {
        _chainBlade = chainBlade;
    }
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        // if (!_initialized)
        // {
        //     _initialized = true;
        //     _chainBlade.ChainbladeHpBarUi.SetActive(true);
        // }
        _chainBlade.NavMeshAgent.isStopped = false;
        _chainBlade.ChainbladeHpBarUi.SetActive(true);
        _chase =_chainBlade.StartCoroutine(_chainBlade.StartChasing("Chase"));
    }

    public void OnExit()
    {
       _chainBlade.StopCoroutine(_chase);
    }
}