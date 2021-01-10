using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredComponent : MonoBehaviour
{
    private ITriggerableObject _triggerableObject;

    private void Awake()
    {
        _triggerableObject = GetComponent<ITriggerableObject>();
        _triggerableObject.OnTriggered += Activate;
    }

    protected abstract void Activate();

    private void OnDestroy()
    {
        _triggerableObject.OnTriggered -= Activate;
    }
}