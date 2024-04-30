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
        private RagdollHandler _ragdollHandler;
        private CapsuleCollider _characterCollider;
        private Rigidbody _characterRigidbody;
        private Animator _animator;
        private Health _health;
        private bool isFall = false;

        public bool IsFall() { return isFall; }

        public void Initialize()
        {
            Debug.Log("RagdollSystem.Initialize()");

            _characterCollider = GetComponent<CapsuleCollider>();
            _characterRigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _health = GetComponent<Health>();

            _ragdollHandler = GetComponentInChildren<RagdollHandler>();
            if (_ragdollHandler == null)
            {
                Debug.LogError("RagdollHandler not found on " + gameObject.name);
                return;
            }

            _ragdollHandler.Disable();
            if (_health != null) _health.onDied.AddListener(Fall);
        }


        public void Fall()
        {
            isFall = true;

            _characterCollider.enabled = false;
            _characterRigidbody.isKinematic = true;
            if (_animator != null) _animator.enabled = false;

            _ragdollHandler.Enable();
        }
    }

}
