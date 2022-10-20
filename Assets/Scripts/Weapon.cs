using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public GameObject model;
    public delegate void WeaponDelegate();
    public WeaponDelegate weaponDelegate;

    public float fireRate;
    public float bulletDamage;
    public int bulletCount;
    public float bulletSpread;
    public float bulletConeLength;
    public float knockbackForce = 50f;
    public EAmmoType ammoType;
}

public enum EAmmoType
{
    Standard,
    Heavy,
    Automatic
}
