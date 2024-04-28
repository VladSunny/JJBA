using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using JJBA.Combat;
namespace JJBA.UI
{
    public class HealthBar : MonoBehaviour
    {
        private float _lerpTimer;

        [SerializeField] private float chipSpeed = 2f;
        [SerializeField] private Health healthComponent;

        public Image frontHealthBar;
        public Image backHealthBar;

        private void Start()
        {     
            healthComponent.onHealthDamaged.AddListener(HealthChanged);
            healthComponent.onHealthHealed.AddListener(HealthChanged);
        }

        private void Update()
        {
            UpdateHealthUI();
        }

        public void UpdateHealthUI()
        {
            float fillF = frontHealthBar.fillAmount;
            float fillB = backHealthBar.fillAmount;
            float hFraction = healthComponent.GetHealth() / healthComponent.GetMaxHealth();

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

        private void HealthChanged(float damage)
        {
            _lerpTimer = 0f;
        }
    }
};