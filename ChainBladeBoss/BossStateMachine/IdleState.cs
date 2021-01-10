public class IdleState : IState
{
    private readonly ChainBlade _chainBlade;
    private bool _initialized;
    public IdleState(ChainBlade chainBlade)
    {
        _chainBlade = chainBlade;
    }
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        if(_initialized)        
            _chainBlade.ResetBoss();
        
        _chainBlade.ChainbladeHpBarUi.SetActive(false);
        _chainBlade.NavMeshAgent.isStopped = true;
        _initialized = true;
        
        
    }

    public void OnExit()
    {
       
    }
}