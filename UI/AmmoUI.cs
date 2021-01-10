using System;
using System.Collections;
using System.Collections.Generic;
using PlayerWeaponSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image weaponSpriteImage;
    private Inventory _inventory;

    private void Awake()
    {
        _inventory = FindObjectOfType<Inventory>();
        _inventory.WeaponFired += InventoryOnWeaponFired;
        _inventory.WeaponEquipped += InventoryOnWeaponEquipped;
    }

    private void InventoryOnWeaponEquipped(Weapon equippedWeapon)
    {
        weaponSpriteImage.sprite = equippedWeapon.WeaponSprite;
        ammoText.text = equippedWeapon.CurrentAmmo + "/" + equippedWeapon.MaxAmmo;
    }


    private void InventoryOnWeaponFired(int currentAmmo,int maxAmmo)
    {
        ammoText.text = currentAmmo + "/" + maxAmmo;
    }
}
