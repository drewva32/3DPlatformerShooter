using System;
using UnityEngine;

public interface ITriggerableObject
{
    event Action OnTriggered;
}