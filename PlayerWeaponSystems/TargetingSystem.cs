using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerWeaponSystems
{
    public class TargetingSystem : MonoBehaviour
    {
        [SerializeField] private float tickSpeed;
        [SerializeField] private float aimRadius;
        [SerializeField] private float maxAimDistance;
        [SerializeField] private Transform originPoint;
        [SerializeField] private LayerMask enemyLayermask;
        [SerializeField] private Image lockOnImage;

        private List<ITargetable> _detectedEnemies = new List<ITargetable>();
        private RaycastHit[] _hits;
        private Camera _cam;
        private Vector3 _positionToTarget;
        private ITargetable _targetedEnemy;
        [SerializeField]private Inventory _inventory;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Start()
        {
            StartCoroutine(FindEnemies());
        }

        private void Update()
        {
            UpdateLockOnImagePosition();
        }

        private IEnumerator FindEnemies()
        {
            while (true)
            {
                _detectedEnemies.Clear();
            
                _hits = Physics.SphereCastAll(originPoint.position, aimRadius, originPoint.forward, maxAimDistance,
                    enemyLayermask);

                for (int i = 0; i < _hits.Length; i++)
                {
                    _detectedEnemies.Add(_hits[i].collider.GetComponent<ITargetable>());
                }

                if (_detectedEnemies.Count > 0)
                {
                    float shortestDistance = 500f;
                    for (int i = 0; i < _detectedEnemies.Count; i++)
                    {
                        var currentDistance = Vector3.Distance(originPoint.position, _detectedEnemies[i].TargetPoint.position);
                        if (currentDistance < shortestDistance)
                        {
                            shortestDistance = currentDistance;
                            _targetedEnemy = _detectedEnemies[i];
                            if(_inventory.CurrentWeapon.TargetsEnemies)
                                _inventory.CurrentWeapon.SetTarget(_targetedEnemy);
                        }
                    }
                    lockOnImage.enabled = true;

                }
                else
                {
                    lockOnImage.enabled = false;
                    _targetedEnemy = null;
                    _inventory.CurrentWeapon.HasTarget = false;
                }

                // Debug.Log(_enemiesInRange.Count);
                yield return new WaitForSeconds(tickSpeed);
            }
        }

        private void UpdateLockOnImagePosition()
        {
            if (_targetedEnemy != null)// !=null
            {
                lockOnImage.transform.position = _cam.WorldToScreenPoint(_targetedEnemy.TargetPoint.position);
            }
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.yellow;
        //     for (int i = 0; i < 4; i++)
        //     {
        //         Gizmos.DrawWireSphere(originPoint.position + originPoint.forward * (8 * i), aimRadius);
        //     }
        // }
    }
}