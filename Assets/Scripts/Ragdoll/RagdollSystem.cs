using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;
namespace JJBA.Ragdoll
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class RagdollSystem : MonoBehaviour
    {
        [Header ("Dependencies")]
        [SerializeField] private Transform _hipsBone;

        [Header ("Debug Keys")]
        [SerializeField] private KeyCode _fallKey = KeyCode.R;
        [SerializeField] private KeyCode _standKey = KeyCode.T;

        private RagdollHandler _ragdollHandler;
        private CapsuleCollider _characterCollider;
        private Rigidbody _characterRigidbody;
        private Animator _animator;
        private Health _health;
        private bool isFall = false;

        public bool IsFall() { return isFall; }

        public void Initialize()
        {
            _characterCollider = GetComponent<CapsuleCollider>();
            _characterRigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _health = GetComponent<Health>();

            Debug.Log(_hipsBone);

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
            if (Input.GetKeyDown(_fallKey)) Fall();
            if (Input.GetKeyDown(_standKey)) Stand();
        }

        public void Fall()
        {
            isFall = true;

            _characterCollider.enabled = false;
            _characterRigidbody.isKinematic = true;
            if (_animator != null) _animator.enabled = false;

            _ragdollHandler.Enable();
        }

        public void Stand()
        {
            isFall = false;

            _ragdollHandler.Disable();

            _characterCollider.enabled = true;
            _characterRigidbody.isKinematic = false;
            if (_animator != null) _animator.enabled = true;
        }

        private void AdjustToHipsPosition()
        {
            transform.position = _hipsBone.position;
        }
    }

}
