using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float _health;
    private float _lerpTimer;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float chipSpeed = 2f;

    public Image frontHealthBar;
    public Image backHealthBar;

    private void Start()
    {
        _health = maxHealth;
    }

    private void Update()
    {
        _health = Mathf.Clamp(_health, 0, maxHealth);
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = _health / maxHealth;
        
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        _health += healAmount;
        _lerpTimer = 0f;
    }
}
