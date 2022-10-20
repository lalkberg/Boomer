using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAwarded = 10;
    public LayerMask pickupableLayer;
    public bool destroyOnPickup = true;
    private bool onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (onCooldown) return;

        if (other.TryGetComponent(out Health health) && other.gameObject.CompareTag("Player"))
        {
            int actualHealthChange = healthAwarded;
            if (health.currentHealth + healthAwarded >= health.health)
            {
                actualHealthChange = health.health - health.currentHealth;
            }
            Debug.Log(actualHealthChange);
            health.ChangeHealth(-actualHealthChange);

            if (destroyOnPickup) gameObject.SetActive(false);
            else
            { 
                StartCoroutine(Cooldown(5f));
            }
        }
    }

    IEnumerator Cooldown(float cooldown)
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
