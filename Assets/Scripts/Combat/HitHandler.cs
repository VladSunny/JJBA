using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Combat
{
    [RequireComponent(typeof(Health))]
    public class HitHandler : MonoBehaviour
    {
        private static readonly int damagedAV = Animator.StringToHash("damaged");

        private Health _health;
        private Animator _animator;

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _animator = GetComponentInChildren<Animator>();

            _health.onHealthDamaged.AddListener(Hitted);
        }

        private void Hitted(float damage)
        {
            _animator.SetTrigger(damagedAV);
        }
    }
}
