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
        [SerializeField] private float strongSlapSpeed = 13f;

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
                _audioManager.Play("Hitted_" + Random.Range(1, 4));
                _animator.SetTrigger(damagedAV);
                _rb.AddForce(damage.forse, ForceMode.Impulse);
                _particleManager.Play("Hitted", damage.forse.normalized);
            }

            if (damage.type == DamageType.SLAP)
            {
                _audioManager.Play("Hitted_" + Random.Range(1, 4));
                if (damage.forse.magnitude >= strongSlapSpeed)
                    _particleManager.Play("Slapped", Vector3.up, new Vector3(0, 0.2f, 0));
            }

            if (damage.type == DamageType.NONE)
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
