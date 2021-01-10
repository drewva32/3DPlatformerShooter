using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerWeaponSystems
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private string weaponName;
        [Header("Prefabs")]
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected GameObject muzzleFlashPrefab;
        [Header("WeaponStats")]
        [SerializeField] private int maxAmmo;
        [SerializeField] private float fireRate;
        [SerializeField] private int ammoPickupAmount;
        [SerializeField] protected Transform[] firePoints;
        [SerializeField] private Transform dynamicFirePoint;

        [Header("Misc")] 
        [Tooltip("check if weapon uses targeting system \n" + "Ex. blaster uses targeting, shotgun does not")]
        [SerializeField] protected bool targetsEnemies;
        [SerializeField] private Sprite weaponSprite;

        [Header("Description")]
        [SerializeField] private string weaponDescription;

        public int CurrentAmmo
        {
            get => _currentAmmo;
            set => _currentAmmo = value;
        }

        public Sprite WeaponSprite => weaponSprite;

        public ITargetable TargetedEnemy { get; set; }
        public bool HasTarget { get; set; }
        public int MaxAmmo => maxAmmo;
        public bool TargetsEnemies => targetsEnemies;

        private Camera _camera;
        private RaycastHit _hitInfo;
        private TaterCharacterController _taterCharacterController;
        private int _currentAmmo;
        private float _coolDownTimer;
        private bool _ismuzzleFlashPrefabNotNull;
        protected Vector3 AimDirection;
        protected bool CanFire;

        private void Awake()
        {
            _taterCharacterController = FindObjectOfType<TaterCharacterController>();
            _camera = Camera.main;
            _currentAmmo = maxAmmo;
            _ismuzzleFlashPrefabNotNull = muzzleFlashPrefab != null;
        }

        public virtual void FireWeapon()
        {
            if (!CanFire)
                return;
            _coolDownTimer = fireRate;
            _currentAmmo--;
            AimDirection = GetDirection();
            SpawnWeaponFiredPrefabs();
        }

        protected  virtual Vector3 GetDirection()
        {
            if (!_taterCharacterController.ToggleStrafe)//always return guns forward direction when not in strafing aim mode.
                return firePoints[0].forward;
        
            if (!targetsEnemies)//always shoot weapon where you are pointing if weapon doesn't use targeting system.
            {
                var ray =_camera.ViewportPointToRay(Vector3.one / 2f);
                Vector3 target = ray.GetPoint(1000);
                return target;
            }

            return dynamicFirePoint.forward;//this transform is rotated by FirePointAimer class
        }

        protected virtual void SpawnWeaponFiredPrefabs()
        {
            Transform myInstance = PoolManager.Pools["Projectiles"].Spawn(projectilePrefab.transform, firePoints[0].position,
                Quaternion.LookRotation(AimDirection));
            if (HasTarget)
            {
                myInstance.gameObject.GetComponent<Projectile>().SetTargetEnemy(TargetedEnemy);
            }
        
            if (_ismuzzleFlashPrefabNotNull)
            {
                Transform newInstance = PoolManager.Pools["WeaponParticles"].Spawn(muzzleFlashPrefab.transform,
                    firePoints[0].position,
                    Quaternion.LookRotation(-firePoints[0].forward, firePoints[0].up));
            }
        }

        protected virtual void Update()
        {
            UpdateCoolDownTimer();
            CheckIfCanFire();
        }
    
        private void UpdateCoolDownTimer()
        {
            _coolDownTimer -= Time.deltaTime;
        }
    
        private void CheckIfCanFire()
        {
            if (_coolDownTimer > 0 || _currentAmmo < 1)
                CanFire = false;
            else
            {
                CanFire = true;
            }
        }

        public void SetTarget(ITargetable enemy)
        {
            TargetedEnemy = enemy;
            HasTarget = true;
        }


    }
}