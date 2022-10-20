using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health ownerHealth;
    public float trailSpeed = 1f;
    public Image healthBarActual;
    public Image healthBarTrail;

    private float healthNormalized;
    private float healthLastCheck = -1;
    private float healthResult;
    private float currentBarFill = 1;
    private float healthTrailStartValue;
    private bool updateBar = false;
    private float trailTimeRemaining =-1;
    // Start is called before the first frame update
    void OnEnable()
    {
        ownerHealth.healthChangeCallback += TakeDamage;
    }

    private void OnDisable()
    {
        ownerHealth.healthChangeCallback -= TakeDamage;
    }

    private void Update()
    {
        if (updateBar)
        {
            currentBarFill = UpdateTrail(healthTrailStartValue, endValueNormalized, trailTimeRemaining);
            trailTimeRemaining -= Time.deltaTime;

            if (trailTimeRemaining <= 0)
            {
                healthLastCheck = -1;
                trailTimeRemaining = -1;
                updateBar = false;
            }
        }
    }

    private float startValueNormalized => healthLastCheck / ownerHealth.health;

    private float endValueNormalized => healthResult / ownerHealth.health;

    void TakeDamage(HealthChangeStruct healthChange)
    {
        healthNormalized = (float)ownerHealth.currentHealth / (float)ownerHealth.health;
        Debug.LogWarning(healthNormalized);
        healthBarActual.fillAmount = healthNormalized;

        if (healthLastCheck < 0)
        {
            healthLastCheck = healthChange.healthAtChange;
        }

        healthResult = healthChange.healthResult;
        trailTimeRemaining = trailSpeed;
        healthTrailStartValue = currentBarFill != startValueNormalized ? currentBarFill : startValueNormalized;
        updateBar = true;
    }

    float UpdateTrail(float startValue, float endValue, float time)
    {
        float barFill = Mathf.Lerp(endValue, startValue, time);
        healthBarTrail.fillAmount = barFill;
        return barFill;
    }
}
