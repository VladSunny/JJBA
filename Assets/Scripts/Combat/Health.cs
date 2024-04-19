using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace JJBA.Combat
{
    public class Health : MonoBehaviour
    {
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

        private void Start()
        {
            health = maxHealth;
        }

        private void Update()
        {
            if (Input.GetKeyDown(decreaseHealth))
                this.Damage(debugDamage);
            if (Input.GetKeyDown(increaseHealth))
                this.Heal(debugHeal);
        }

        public void Damage(float damage)
        {
            health = Mathf.Max(0, health - damage);
        }

        public void Heal(float heal)
        {
            health = Mathf.Min(health + heal, maxHealth);
        }
    }
}
