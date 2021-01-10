using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Button : MonoBehaviour
{
    [SerializeField] private UnityEvent onButtonPushed;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            onButtonPushed?.Invoke();
    }
}
