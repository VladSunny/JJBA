using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.UI;
using JJBA.Core;
using JJBA.Combat.Events;
using JJBA.Movement;
using JJBA.Audio;

namespace JJBA.Combat
{
    [RequireComponent(typeof(DynamicHitBox))]
    [RequireComponent(typeof(Rigidbody))]

    public class StandlessFighter : MonoBehaviour
    {
        [Header("Base Punches settings")]
        [SerializeField] private BasePunchesConfig basePunchesConfig;

        [Header("Dependencies")]
        public Transform characterTransform;

        [Header("Debug")]
        [SerializeField] private bool drawHitBox;
        [SerializeField]
        [SeeOnly]
        private int _basePunchCounter = 0;

        private DynamicHitBox _dynamicHitBox;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private StandlessEvents _standlessEvents;
        private AudioManager _audioManager;
        private Mover _mover;
        private CooldownUIManager _cooldownUIManager;

        private static readonly int basePunchesNumberAV = Animator.StringToHash("basePunchesNumber");
        private static readonly int punchAV = Animator.StringToHash("basePunch");

        private float _basePunchComboTimer = 0f;
        private bool _readyToPunch = true;

        public void Initialize()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _dynamicHitBox = GetComponentInChildren<DynamicHitBox>();
            _standlessEvents = GetComponentInChildren<StandlessEvents>();
            _mover = GetComponent<Mover>();
            _standlessEvents.onBasePunch.AddListener(DoPunch);
            _audioManager = GetComponentInChildren<AudioManager>();
            _cooldownUIManager = GetComponent<CooldownUIManager>();
        }

        private void Update()
        {
            if (_basePunchCounter > 0)
            {
                if (_basePunchComboTimer <= basePunchesConfig.basePunchComboTime)
                {
                    _basePunchComboTimer += Time.deltaTime;
                }
                else
                {
                    _basePunchCounter = 0;
                    _basePunchComboTimer = 0f;
                    _readyToPunch = false;
                    Invoke(nameof(ResetPunch), basePunchesConfig.basePunchComboCooldown);
                    if (_cooldownUIManager != null)
                        _cooldownUIManager.AddCooldownTimer(basePunchesConfig.basePunchComboCooldown, "Base Punch");
                }
            }
        }

        public void BasePunch()
        {
            if (!_readyToPunch) return;

            if (_mover != null && basePunchesConfig.stopRunning) _mover.SetRunning(false);

            _animator.SetFloat(basePunchesNumberAV, (float)(_basePunchCounter % 2));

            _animator.SetTrigger(punchAV);

            _audioManager.Play("BasePunch_" + (_basePunchCounter % 2 + 1));

            if (_basePunchComboTimer < basePunchesConfig.basePunchComboTime)
            {
                _basePunchCounter++;
                _basePunchComboTimer = 0f;
            }

            _readyToPunch = false;

        }

        private void DoPunch()
        {
            _dynamicHitBox.CreateHitBox(Vector3.forward * 1f, new Vector3(1f, 1f, 1f), (collider) =>
            {
                if (collider.transform == this.transform || !(collider is CapsuleCollider capsuleCollider))
                    return;

                Health enemyHealth = collider.transform.GetComponent<Health>();
                Rigidbody enemyRigidbody = collider.transform.GetComponent<Rigidbody>();
                Damage damage;

                if (_basePunchCounter == basePunchesConfig.basePunchesNumber)
                {
                    damage = new()
                    {
                        damageValue = basePunchesConfig.finisherBasePunchDamage,
                        from = characterTransform.position,
                        forse = characterTransform.forward * basePunchesConfig.finisherBasePunchForce,
                        type = DamageType.PUNCH_FINISHER
                    };
                }
                else
                {
                    damage = new()
                    {
                        damageValue = basePunchesConfig.basePunchDamage,
                        from = characterTransform.position,
                        forse = characterTransform.forward * basePunchesConfig.basePunchForce,
                        type = DamageType.BASE
                    };
                }

                if (enemyHealth != null)
                    collider.transform.GetComponent<Health>().GetDamage(damage);
            }, drawHitBox);

            if (_basePunchCounter >= basePunchesConfig.basePunchesNumber)
            {
                if (_cooldownUIManager != null)
                    _cooldownUIManager.AddCooldownTimer(basePunchesConfig.basePunchComboCooldown, "Base Punch");
                _basePunchCounter = 0;
                Invoke(nameof(ResetPunch), basePunchesConfig.basePunchComboCooldown);
            }
            else
            {
                Invoke(nameof(ResetPunch), basePunchesConfig.basePunchCooldown);
            }
        }

        private void ResetPunch()
        {
            _readyToPunch = true;
        }
    }
}

