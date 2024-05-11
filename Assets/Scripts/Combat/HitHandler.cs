using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Audio;

namespace JJBA.Combat
{
    [RequireComponent(typeof(Health))]
    public class HitHandler : MonoBehaviour
    {
        private static readonly int damagedAV = Animator.StringToHash("damaged");

        private Health _health;
        private Animator _animator;
        private Rigidbody _rb;
        private AudioManager _audioManager;

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _animator = GetComponentInChildren<Animator>();
            _audioManager = GetComponentInChildren<AudioManager>();
            _rb = GetComponent<Rigidbody>();

            _health.onHealthDamaged.AddListener(Hitted);
            _health.onDied.AddListener(OnDeath);
        }

        private void Hitted(Damage damage)
        {
            if (damage.type == DamageType.BASE)
            {
                _animator.SetTrigger(damagedAV);
                _rb.AddForce(damage.forse, ForceMode.Impulse);
            }

            if (damage.type == DamageType.BASE || damage.type == DamageType.NONE)
            {
                _audioManager.Play("Hitted_" + Random.Range(1, 4));
            }
        }

        private void OnDeath()
        {
            _audioManager.Play("Death");
        }
    }
}
