using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Audio;
using JJBA.VFX;
using UnityEngine.Animations;

namespace JJBA.Combat
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]

    public class HitHandler : MonoBehaviour
    {
        private static readonly int damagedAV = Animator.StringToHash("damaged");

        private Health _health;
        private Animator _animator;
        private Rigidbody _rb;
        private AudioManager _audioManager;
        private ParticleManager _particleManager;

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _animator = GetComponentInChildren<Animator>();
            _audioManager = GetComponentInChildren<AudioManager>();
            _particleManager = GetComponentInChildren<ParticleManager>();
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
                _particleManager.Play("Hitted", damage.forse.normalized);
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
