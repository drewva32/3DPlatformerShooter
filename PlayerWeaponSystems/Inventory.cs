using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerWeaponSystems
{
    public class Inventory : MonoBehaviour
    {
        public Weapon[] weapons;
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private AudioClip noAmmoSound;
        public Weapon CurrentWeapon => _currentWeapon;
        public event Action<int,int> WeaponFired;
        public event Action<Weapon> WeaponEquipped;
        
        private Weapon _currentWeapon;
        private Weapon _previousWeapon;
        private int _index;
        private Ratchet _ratchet;
        private AudioSource _audioSource;


        public void EquipWeapon(Weapon weaponToEquip)
        {
            if (_currentWeapon != null)
                _currentWeapon.gameObject.SetActive(false);

            _currentWeapon = weaponToEquip;
            weaponToEquip.transform.parent = weaponHolder.transform;
            weaponToEquip.transform.localPosition = Vector3.zero;
            weaponToEquip.transform.localRotation = Quaternion.identity;
            weaponToEquip.gameObject.SetActive(true);
            WeaponEquipped?.Invoke(_currentWeapon);
        }

        private void OnEnable()
        {
            _ratchet = GetComponent<Ratchet>();
            _audioSource = GetComponent<AudioSource>();
            EquipWeapon(weapons[0]);
        }

        public void MaxAmmoAllWeapons()
        {
            foreach (var weapon in weapons)
            {
                weapon.CurrentAmmo = weapon.MaxAmmo;
            }
            WeaponEquipped?.Invoke(_currentWeapon);
        }

        private void Update()
        {
            // if(Keyboard.current.digit1Key.wasPressedThisFrame)
            //     EquipWeapon(weapons[0]);
            // if(Keyboard.current.digit2Key.wasPressedThisFrame)
            //     EquipWeapon(weapons[1]);
            // if(Keyboard.current.digit3Key.wasPressedThisFrame)
            //     EquipWeapon(weapons[2]);
            // if(Keyboard.current.digit4Key.wasPressedThisFrame)
            //     EquipWeapon(weapons[3]);
            // if(Keyboard.current.digit5Key.wasPressedThisFrame)
            //     EquipWeapon(weapons[4]);
        
            if(Gamepad.current.buttonNorth.wasPressedThisFrame)
                SwapWeapon();
                
            if (_ratchet.Dead)
                return;
            if (Gamepad.current.rightShoulder.isPressed)
            {
                if (_currentWeapon.CurrentAmmo < 1)
                {
                    DAudio.PlayClip(noAmmoSound,_audioSource,1,1);
                }
                _currentWeapon.FireWeapon();
                WeaponFired?.Invoke(_currentWeapon.CurrentAmmo,_currentWeapon.MaxAmmo);
            }
        }

        private void SwapWeapon()
        {
            _index++;
            if (_index >= weapons.Length)
                _index = 0;
            EquipWeapon(weapons[_index]);
        }
    }
}
