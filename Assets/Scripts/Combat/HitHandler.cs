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
        private AudioManager _audioManager;

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _animator = GetComponentInChildren<Animator>();
            _audioManager = GetComponentInChildren<AudioManager>();

            _health.onHealthDamaged.AddListener(Hitted);
        }

        private void Hitted(float damage)
        {
            _animator.SetTrigger(damagedAV);
            _audioManager.Play("Hitted_" + Random.Range(1, 4));
        }
    }
}
