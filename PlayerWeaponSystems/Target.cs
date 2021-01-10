using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour,IGetShot
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void GetShot(float damage,Vector3 positionOfDamager)
    {
        _rb.AddForce(Vector3.up * 300);
    }
    
    
}
