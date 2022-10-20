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
    public Image healthBarTrailHeal;

    private float healthNormalized;
    private float healthLastCheck = -1;
    private float healthResult;
    private float currentBarFill = 1;
    private float healthTrailStartValue;
    private bool updateBar = false;
    private bool damaging;
    private float trailTimeRemaining =-1;
    private float startValueNormalized => healthLastCheck / ownerHealth.health;

    private float endValueNormalized => healthResult / ownerHealth.health;
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
            Image bar = damaging ? healthBarTrail : healthBarActual;
            currentBarFill = UpdateTrail(healthTrailStartValue, endValueNormalized, trailTimeRemaining, bar);
            trailTimeRemaining -= Time.deltaTime;

            if (trailTimeRemaining <= 0)
            {
                healthLastCheck = -1;
                trailTimeRemaining = -1;
                updateBar = false;
            }
        }
    }

    void TakeDamage(HealthChangeStruct healthChange)
    {
        healthNormalized = (float)healthChange.healthResult / (float)ownerHealth.health;
        //Debug.LogWarning(healthNormalized);
        damaging = healthChange.isDamaging;

        if (damaging)
        {
            healthBarActual.fillAmount = healthNormalized;
        }

        if (!damaging)
        {
            Debug.Log(healthNormalized);
        }

        healthBarTrailHeal.fillAmount = healthNormalized;

        if (healthLastCheck < 0)
        {
            healthLastCheck = healthChange.healthBeforeChange;
        }

        healthResult = healthChange.healthResult;
        trailTimeRemaining = trailSpeed;
        healthTrailStartValue = currentBarFill != startValueNormalized ? currentBarFill : startValueNormalized;
        updateBar = true;
    }

    float UpdateTrail(float startValue, float endValue, float time, Image bar)
    {
        float barFill = Mathf.Lerp(endValue, startValue, time);
        bar.fillAmount = Mathf.Clamp(barFill, 0, 1);
        return barFill;
    }
}
