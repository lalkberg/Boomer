using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public delegate void HealthChangeCallback(HealthChangeStruct healthChange);
    public HealthChangeCallback healthChangeCallback;
    public UnityEvent onDeath;
    public UnityEvent onDamage;

    public int health = 100;
    public int currentHealth { get; private set; }
    private Coroutine knockbackCoroutine;
    private const float knockbackCooldown = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        ChangeHealth(0);
    }

    private void OnEnable()
    {
        healthChangeCallback += Death;
    }

    public void TakeDamage(float damage, Vector3 knockback)
    {
        onDamage?.Invoke();
        HealthChangeStruct change = ChangeHealth(damage);
        if (change.lethal) return;

        knockbackCoroutine ??= StartCoroutine(KnockbackTimer(knockback)); // lol tydligen en null check assignment???
    }

    IEnumerator KnockbackTimer(Vector3 knockback)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = knockback;
        yield return new WaitForSeconds(knockbackCooldown);

        knockbackCoroutine = null;
    }

    public HealthChangeStruct ChangeHealth(float change)
    {
        HealthChangeStruct healthChange = new(currentHealth, Mathf.CeilToInt(change));
        currentHealth = healthChange.healthResult;

        Debug.Log($"{gameObject.name} took {change} damage.");
        if (healthChange.lethal) Debug.Log("Lethal!");
        else Debug.Log("Current health: " + currentHealth);

        healthChangeCallback(healthChange);
        return healthChange;
    }

    private void Death(HealthChangeStruct healthChange)
    {
        if (!healthChange.lethal) return;
        Debug.Log(gameObject.name + " died!");
        onDeath?.Invoke();
    }
}

public struct HealthChangeStruct
{
    public int healthBeforeChange;
    public int healthResult;
    public int healthDifference;
    public bool lethal;
    public bool isDamaging => healthDifference >= 0;

    public HealthChangeStruct(int current, int difference)
    {
        healthBeforeChange = current;
        healthDifference = difference;
        healthResult = Mathf.Max(current - difference, 0);
        lethal = healthResult <= 0;
    }
}
