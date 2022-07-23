using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoadout : MonoBehaviour
{
    [SerializeField] private Weapon _startingWeaponPrefab;
    [SerializeField]
    [Tooltip("Location where weapon should be held, positioned at" +
        " hand location")]
    private Transform _weaponSocket;

    public Weapon EquippedWeapon { get; private set; }

    public void Awake()
    {
        if (_startingWeaponPrefab != null)
            EquipWeapon(_startingWeaponPrefab);
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") 
            && EquippedWeapon.IsOnCooldown == false)
        {
            ShootWeapon();
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        if(EquippedWeapon != null)
        {
            Destroy(EquippedWeapon.gameObject);
        }
        // spawn weapon in the world and remember it
        EquippedWeapon = Instantiate
            (newWeapon, _weaponSocket.position, _weaponSocket.rotation);
        EquippedWeapon.transform.SetParent(_weaponSocket);
    }
    
    public void ShootWeapon()
    {
        // no matter what weapon we have, do its own Shoot()
        EquippedWeapon.Shoot();
    }
}
