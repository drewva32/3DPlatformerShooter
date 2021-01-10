using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float lerpDuration = 4f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private bool moveOnlyOnce;
    [SerializeField] private bool triggeredByEvent;
    
    private Rigidbody _rb;
    private bool _currentlyMoving;
    private bool _doneMoving;
    private Vector3 _position;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _position = transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (triggeredByEvent)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            StartMoving();
        }
    }

    public void StartMoving()
    {
        if (_currentlyMoving || _doneMoving)
            return;
        StartCoroutine(MovePlatform());
    }


    private IEnumerator MovePlatform()
    {
        
        _currentlyMoving = true;
        
        yield return new WaitForSeconds(0.15f);
        float timer = 0;
        while (timer < lerpDuration)
        {
            _position = Vector3.Lerp(startPoint.position, endPoint.position, curve.Evaluate(timer / lerpDuration));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (moveOnlyOnce)
            _doneMoving = true;
        _currentlyMoving = false;
    }

    private void FixedUpdate()
    {
        if (!_currentlyMoving)
            return;
        _rb.MovePosition(_position);
    }
}

