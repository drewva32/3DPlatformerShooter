using System.Linq;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private CheckPoint[] _checkpoints;

    private void Start()
    {
        _checkpoints = GetComponentsInChildren<CheckPoint>();
    }

    public CheckPoint GetLastCheckPointThatWasPassed()
    {
        return _checkpoints.Last(t => t.Passed);
    }
}