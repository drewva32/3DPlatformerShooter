using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using PlayerWeaponSystems;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BombLauncherWeapon : Weapon
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private GameObject bombGroundMarkerPrefab;
    public List<Vector3> lineRenderPoints;
    private RaycastHit _hit;
    private Vector3 _gizmoPosition;
    private GameObject _bombGroundMarker;
    private Vector3 _rotatedDirection;

    private void OnEnable()
    {
        _bombGroundMarker = Instantiate(bombGroundMarkerPrefab);
    }

    protected override void SpawnWeaponFiredPrefabs()
    {
        Transform myInstance = PoolManager.Pools["Projectiles"].Spawn(projectilePrefab.transform,
            firePoints[0].position,
            Quaternion.LookRotation(_rotatedDirection));

        Transform newInstance = PoolManager.Pools["WeaponParticles"].Spawn(muzzleFlashPrefab.transform,
            firePoints[0].position,
            Quaternion.LookRotation(-firePoints[0].forward, firePoints[0].up));
    }

    private void LateUpdate()
    {
        _rotatedDirection = Quaternion.AngleAxis(-45, firePoints[0].right) * (firePoints[0].forward * 1.5f);
        if (Ext_Physics.CurveCast(firePoints[0].position, _rotatedDirection, Vector3.down * 10f, .8f, out _hit, 100f,
            out lineRenderPoints, aimLayerMask))
        {
            _gizmoPosition = _hit.point;
            _bombGroundMarker.transform.position = _hit.point + (_hit.normal * 0.1f);
            _bombGroundMarker.transform.up = Quaternion.Euler(90, 0, 0) * _hit.normal;
            lineRenderer.positionCount = lineRenderPoints.Count;
            for (int i = 0; i < lineRenderPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, lineRenderPoints[i]);
            }
        }

        _bombGroundMarker.transform.Rotate(_bombGroundMarker.transform.up, 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_gizmoPosition, 2f);
    }
}