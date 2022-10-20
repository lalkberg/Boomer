using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : CharacterShoot
{
    private void Start()
    {
        currentWeapon = allWeapons[0];
    }
}
