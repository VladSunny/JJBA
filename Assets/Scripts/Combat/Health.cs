using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;


namespace JJBA.Combat
{
    public class Health : MonoBehaviour
    {
        public UnityEvent<float> onHealthDamaged;
        public UnityEvent<float> onHealthHealed;
        public UnityEvent onDied;

        [Header("Settings")]
        [SerializeField] private float maxHealth = 100f;

        [Header("Debug")]
        [SerializeField] private float debugDamage = 10f;
        [SerializeField] private float debugHeal = 5f;
        [SerializeField] private KeyCode decreaseHealth = KeyCode.Alpha1;
        [SerializeField] private KeyCode increaseHealth = KeyCode.Alpha2;

        [Header("Health")]
        [SerializeField]
        [SeeOnly]
        private float health;

        public float GetHealth() => health;
        public float GetMaxHealth() => maxHealth;


        public void Initialize()
        {
            health = maxHealth;
        }

        private void Start()
        {
            // Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(decreaseHealth))
                Damage(debugDamage);
            if (Input.GetKeyDown(increaseHealth))
                Heal(debugHeal);
        }

        public void Damage(float damage)
        {
            health = Mathf.Max(0, health - damage);
            onHealthDamaged.Invoke(damage);

            if (health <= 0) Die();
        }

        public void Heal(float heal)
        {
            health = Mathf.Min(health + heal, maxHealth);
            onHealthHealed.Invoke(heal);
        }

        private void Die()
        {
            onDied.Invoke();
        }
    }
}
