using System;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using PlayerWeaponSystems;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotGunBlastWeapon : Weapon
{
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float damage;
    
    // private HashSet<IGetShot> _enemiesToDammage = new HashSet<IGetShot>();
    private RaycastHit[] hits = new RaycastHit[15];
    
    // private Vector3 debugDirection;
    // private Vector3 _gizmoPosition;

    public override void FireWeapon()
    {
        base.FireWeapon();
        if (!CanFire)
            return;

        var startPositionOffset = AimDirection.normalized * 2.6f;
        hits = Physics.SphereCastAll(firePoints[0].transform.position + startPositionOffset, 4f, AimDirection, 22f, enemies);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].collider.GetComponent<IGetShot>().GetShot(damage, transform.position);
        }
        //////////////////////////////////////old code that used raycastall instead of spherecastall - can probably delete but I'm shy
            // float numberOfRays = 12f;
        //
        // for (int x = 0; x < 4; x++)
        // {
        //     AimDirection = Quaternion.AngleAxis(0 + (45 * x),Vector3.forward) * AimDirection;
        //
        //     for (int y = 0; y < numberOfRays; y++)
        //     {
        //         var rayDirection =
        //             Quaternion.Euler(0, -18 + (3 * y), 0) *
        //             AimDirection; //creates a fan of raycasts from -15 degrees to 15 degrees incremented by 3 degrees each loop
        //         hits = Physics.RaycastAll(firePoints[0].transform.position, rayDirection, 22f, enemies);
        //         for (int i = 0; i < hits.Length; i++)
        //         {
        //             _enemiesToDammage.Add(hits[i].collider.GetComponent<IGetShot>());
        //         }
        //     }
        // }
        /////////////////////////////////////////

        // foreach (var enemy in _enemiesToDammage)
        // {
        //     enemy.GetShot();
        // }
        //
        // _enemiesToDammage.Clear();
    }

    // private void LateUpdate()
    // {
    //     _gizmoPosition = firePoints[0].transform.position + AimDirection.normalized * 2.5f;
    // }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere( _gizmoPosition,4);
    //     Gizmos.DrawRay(firePoints[0].position,AimDirection);
    // }
}