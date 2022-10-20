using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour
{
    public Weapon[] allWeapons = new Weapon[6];
    public LayerMask shootableLayers;
    internal Weapon currentWeapon;
    private bool fireReady = true;

    public void OnShoot()
    {
        if (fireReady)
        {
            StartCoroutine(Fire());
        }
    }
    internal IEnumerator Fire()
    {
        fireReady = false;
        foreach (RaycastHit hit in ShotsFired())
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Health health))
                {
                    health.TakeDamage(currentWeapon.bulletDamage, Vector3.Normalize(health.transform.position - transform.position) * currentWeapon.knockbackForce);
                }
            }
        }
        yield return new WaitForSeconds(currentWeapon.fireRate);
        fireReady = true;
    }
    internal RaycastHit[] ShotsFired()
    {
        List<RaycastHit> hitList = new();
        for (int i = 0; i < currentWeapon.bulletCount; i++)
        {
            Vector3 spread = Random.insideUnitCircle * currentWeapon.bulletSpread;
            Ray rayFrom = new(transform.position, transform.forward + spread);
            Debug.DrawRay(rayFrom.origin, rayFrom.direction * 10, Color.red, 10f);
            bool hitBool = Physics.Raycast(rayFrom, out RaycastHit hit, Mathf.Infinity, shootableLayers);
            if (hitBool)
            {
                Debug.DrawRay(rayFrom.origin, rayFrom.direction * 10, Color.cyan, 10f);
            }
            else
            {
                Debug.DrawRay(rayFrom.origin, rayFrom.direction * 10, Color.red, 10f);
            }
            hitList.Add(hit);
        }
        return hitList.ToArray();
    }

}
