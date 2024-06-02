using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;
using JJBA.Core;
using UnityEngine.Events;
namespace JJBA.Ragdoll
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class RagdollSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float standDelay = 2f;

        [Header("Dependencies")]
        public Transform hipsBone;
        [SerializeField] private LayerMask groundLayer;

        [Header("Debug Keys")]
        [SerializeField] private bool debugMode = false;
        [SerializeField] private KeyCode _fallKey = KeyCode.R;
        [SerializeField] private KeyCode _standKey = KeyCode.T;

        [HideInInspector] public UnityEvent fallEvent;

        private RagdollHandler _ragdollHandler;
        private CapsuleCollider _characterCollider;
        private Rigidbody _characterRigidbody;
        private Animator _animator;
        private Health _health;
        private GroundCheck _hipsGroundCheck;

        private bool isFall = false;
        private float activateCharacterDelay = 0.2f;
        [SerializeField] private float standTimer = 0f;

        public bool IsFall() { return isFall; }

        public void Initialize()
        {
            _characterCollider = GetComponent<CapsuleCollider>();
            _characterRigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _health = GetComponent<Health>();
            _hipsGroundCheck = hipsBone.GetComponent<GroundCheck>();

            _ragdollHandler = GetComponentInChildren<RagdollHandler>();
            if (_ragdollHandler == null)
            {
                Debug.LogError("RagdollHandler not found on " + gameObject.name);
                return;
            }

            _ragdollHandler.Disable();
            if (_health != null) _health.onDied.AddListener(Fall);
        }

        private void Update()
        {
            if (!_hipsGroundCheck.IsGrounded() || !isFall || _health.IsDead())
             standTimer = 0f;

            if (isFall)
            {
                if (_hipsGroundCheck.IsGrounded())
                    standTimer += Time.deltaTime;

                if (standTimer >= standDelay) Stand();
            }
            
            if (debugMode)
            {
                if (Input.GetKeyDown(_fallKey)) Fall();
                if (Input.GetKeyDown(_standKey)) Stand();
            }
        }

        public void Fall()
        {
            fallEvent.Invoke();
            
            _characterCollider.enabled = false;
            _characterRigidbody.isKinematic = true;
            if (_animator != null) _animator.enabled = false;

            _ragdollHandler.Enable();

            isFall = true;
        }

        public void Stand()
        {
            AdjustPositionToHipsBone();

            isFall = false;

            Invoke(nameof(ActivateCharacter), activateCharacterDelay);

        }

        private void ActivateCharacter()
        {
            _ragdollHandler.Disable();
            _characterCollider.enabled = true;
            _characterRigidbody.isKinematic = false;
            if (_animator != null) _animator.enabled = true;
        }

        private void AdjustPositionToHipsBone()
        {
            Vector3 initHipsPosition = hipsBone.position;
            transform.position = initHipsPosition;

            hipsBone.position = initHipsPosition;
        }

        private void AdjustRotationToHipsBone()
        {
            Vector3 initHipsPosition = hipsBone.position;
            Quaternion initHipsRotation = hipsBone.rotation;

            Vector3 directionForRotate = -hipsBone.up;
            directionForRotate.y = 0;

            Quaternion correctionRotation = Quaternion.FromToRotation(transform.forward, directionForRotate.normalized);
            transform.rotation *= correctionRotation;

            hipsBone.position = initHipsPosition;
            hipsBone.rotation = initHipsRotation;
        }
    }

}
