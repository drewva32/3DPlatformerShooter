using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointAimer : MonoBehaviour
{
    [SerializeField] private float maxAimingDistance = 100f;
    [SerializeField] private LayerMask aimingLayerMask;
    [SerializeField] private Transform staticFirePoint;
    
    private TaterCharacterController _taterCharacterController;
    private RaycastHit _hitInfo;
    private Camera _camera;
    private Transform _transform;

    private void Awake()
    {
        _taterCharacterController = GetComponentInParent<TaterCharacterController>();
        _camera = Camera.main;
        _transform = transform;
    }

    private void Update()
    {
        if (!_taterCharacterController.ToggleStrafe)
            return;
        var ray =_camera.ViewportPointToRay(Vector3.one / 2f);
        Vector3 target = ray.GetPoint(200);
        
        if (Physics.Raycast(ray, out _hitInfo, maxAimingDistance, aimingLayerMask,QueryTriggerInteraction.Ignore))
        {
            target = _hitInfo.point;
        }

        Vector3 direction = target - _transform.position;
        float behindCheck = Vector3.Dot(direction, staticFirePoint.forward);
        if (behindCheck < 0
        ) //if behindCheck is negative then direction was pointing behind character, and needs to be replaced with a default direction.
            direction = ray.GetPoint(1000);
        _transform.forward = direction;
    }
}
