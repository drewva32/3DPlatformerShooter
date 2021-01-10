public class DeadState : IState
{
    private readonly ChainBlade _chainBlade;

    public DeadState(ChainBlade chainBlade)
    {
        _chainBlade = chainBlade;
    }
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        _chainBlade.DisableSword();
        _chainBlade.StartDeath("Die");
        _chainBlade.DemoCompleteField.enabled = true;
        _chainBlade.NavMeshAgent.isStopped = true;
        if(_chainBlade.VerticalSlashSpawner != null)
            _chainBlade.StopCoroutine(_chainBlade.VerticalSlashSpawner);
        _chainBlade.StartCoroutine(_chainBlade.WaitThenDisableSword());
    }

    public void OnExit()
    {
       
    }
}