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
        public UnityEvent<Damage> onHealthDamaged;
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
        private bool isDead = false;

        public float GetHealth() => health;
        public float GetMaxHealth() => maxHealth;
        public bool IsDead() => isDead;


        public void Initialize()
        {
            health = maxHealth;
        }

        private void Update()
        {
            Damage testDamage = new()
            {
                damageValue = debugDamage,
            };

            if (Input.GetKeyDown(decreaseHealth))
                GetDamage(testDamage);
            if (Input.GetKeyDown(increaseHealth))
                Heal(debugHeal);
        }

        public void GetDamage(Damage damage)
        {
            if (health <= 0) return;

            health = Mathf.Max(0, health - damage.damageValue);
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
            isDead = true;
        }
    }
}
