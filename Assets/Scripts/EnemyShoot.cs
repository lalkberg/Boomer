using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : CharacterShoot
{
    public Transform weaponSlot;
    private void Start()
    {
        currentWeapon = allWeapons[Random.Range(0, allWeapons.Length)]; // random.range är max exclusive, corre-kun
        Instantiate(currentWeapon.model, weaponSlot);
    }
}
