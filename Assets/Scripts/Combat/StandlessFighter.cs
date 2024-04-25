using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using JJBA.Core;
using JJBA.Combat.Events;
using JJBA.Combat;

namespace JJBA.Combat
{
    [RequireComponent(typeof(DynamicHitBox))]
    [RequireComponent(typeof(Rigidbody))]

    public class StandlessFighter : MonoBehaviour
    {
        [Header("Base Punches settings")]
        [SerializeField] private float basePunchCooldown = 0.5f;
        [SerializeField] private float basePunchComboCooldown = 2f;
        [SerializeField] private float basePunchComboTime = 1f;
        [SerializeField] private int basePunchesNumber = 5;
        [SerializeField] private float basePunchForce = 20f;
        [SerializeField] private float basePunchMovementForce = 10f;

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

        private static readonly int BasePunchesNumberAV = Animator.StringToHash("basePunchesNumber");
        private static readonly int PunchAV = Animator.StringToHash("basePunch");

        private float _basePunchComboTimer = 0f;
        private bool _readyToPunch = true;

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _dynamicHitBox = GetComponentInChildren<DynamicHitBox>();
            _standlessEvents = GetComponentInChildren<StandlessEvents>();

            _standlessEvents.onBasePunch.AddListener(DoPunch);
        }

        private void Update() {
            if (_basePunchCounter > 0)
            {
                if (_basePunchComboTimer <= basePunchComboTime + 0.1f)
                {
                    _basePunchComboTimer += Time.deltaTime;
                }
                else
                {
                    _basePunchCounter = 0;
                    _basePunchComboTimer = 0f;
                    _readyToPunch = false;
                    Invoke(nameof(ResetPunch), basePunchComboCooldown);
                }
            }
        }

        public void BasePunch()
        {
            if (!_readyToPunch) return;

            _animator.SetFloat(BasePunchesNumberAV, (float)(_basePunchCounter % 2));

            _animator.SetTrigger(PunchAV);

            if (_basePunchComboTimer < basePunchComboTime)
            {
                _basePunchCounter++;
                _basePunchComboTimer = 0f;
            }

            _readyToPunch = false;

            if (_basePunchCounter >= basePunchesNumber)
            {
                _basePunchCounter = 0;
                Invoke(nameof(ResetPunch), basePunchComboCooldown);
            }
            else
            {
                Invoke(nameof(ResetPunch), basePunchCooldown);
            }
        }

        private void DoPunch() {
            _dynamicHitBox.CreateHitBox(Vector3.forward * 1f, new Vector3(1f, 1f, 1f), (collider) =>
            {
                if (collider.transform == this.transform) return;

                Debug.Log(collider);

                Health enemyHealth = collider.transform.GetComponent<Health>();
                Rigidbody enemyRigidbody = collider.transform.GetComponent<Rigidbody>();

                if (enemyHealth != null)
                    collider.transform.GetComponent<Health>().Damage(10f);

                if (enemyRigidbody != null)
                    enemyRigidbody.AddForce(characterTransform.forward * basePunchForce, ForceMode.Impulse);

                _rigidbody.AddForce(characterTransform.forward * basePunchMovementForce, ForceMode.Impulse);

            }, drawHitBox);
        }

        private void ResetPunch() {
            _readyToPunch = true;
        }
    }
}
