using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckPoint : MonoBehaviour
{
    private Collider _collider;
    public bool Passed { get; private set; }

    private void OnValidate()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Ratchet>();
        if (player)
            Passed = true;
    }
}