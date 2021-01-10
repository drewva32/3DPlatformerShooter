using UnityEngine;

public class BlockingState : IState
{
    private readonly ChainBlade _chainBlade;

    public BlockingState(ChainBlade chainBlade)
    {
        _chainBlade = chainBlade;
    }

    private Coroutine _block;
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        _block = _chainBlade.StartCoroutine(_chainBlade.StartBlocking("Blocking"));
    }

    public void OnExit()
    {
        _chainBlade.StopCoroutine(_block);
       _chainBlade.StopBlocking("Blocking");
    }
}