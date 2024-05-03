using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using JJBA.Core;
using JJBA.Combat.Events;
using JJBA.Movement;

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
        private Mover _mover;

        private static readonly int BasePunchesNumberAV = Animator.StringToHash("basePunchesNumber");
        private static readonly int PunchAV = Animator.StringToHash("basePunch");

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
        }

        void Start()
        {
            // Initialize();
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
                }
            }
        }

        public void BasePunch()
        {
            if (!_readyToPunch) return;

            if (_mover != null && basePunchesConfig.stopRunning) _mover.SetRunning(false);

            _animator.SetFloat(BasePunchesNumberAV, (float)(_basePunchCounter % 2));

            _animator.SetTrigger(PunchAV);

            if (_basePunchComboTimer < basePunchesConfig.basePunchComboTime)
            {
                _basePunchCounter++;
                _basePunchComboTimer = 0f;
            }

            _readyToPunch = false;

            if (_basePunchCounter >= basePunchesConfig.basePunchesNumber)
            {
                _basePunchCounter = 0;
                Invoke(nameof(ResetPunch), basePunchesConfig.basePunchComboCooldown);
            }
            else
            {
                Invoke(nameof(ResetPunch), basePunchesConfig.basePunchCooldown);
            }
        }

        private void DoPunch()
        {
            _dynamicHitBox.CreateHitBox(Vector3.forward * 1f, new Vector3(1f, 1f, 1f), (collider) =>
            {
                if (collider.transform == this.transform || !(collider is CapsuleCollider capsuleCollider))
                 return;

                Debug.Log(collider);

                Health enemyHealth = collider.transform.GetComponent<Health>();
                Rigidbody enemyRigidbody = collider.transform.GetComponent<Rigidbody>();

                if (enemyHealth != null)
                    collider.transform.GetComponent<Health>().Damage(basePunchesConfig.basePunchDamage);

                if (enemyRigidbody != null)
                    enemyRigidbody.AddForce(
                        characterTransform.forward * basePunchesConfig.basePunchForce,
                        ForceMode.Impulse);
            }, drawHitBox);
        }

        private void ResetPunch()
        {
            _readyToPunch = true;
        }
    }
}

