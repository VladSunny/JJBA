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
        [SerializeField] private float standDelay = 0.1f;

        [Header("Dependencies")]
        public Transform hipsBone;
        [SerializeField] private LayerMask groundLayer;

        [Header("Debug Keys")]
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

            Invoke(nameof(ActivateCharacter), standDelay);

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

        // private void AdjustPositionRelativeGround()
        // {
        //     Debug.Log("AdjustPositionRelativeGround");
        //     if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), Vector3.down, out RaycastHit hit, 0.8f, groundLayer))
        //     {
        //         Debug.Log("AdjustPositionRelativeGround: " + hit.point);
        //         Debug.DrawLine(transform.position - new Vector3(0, 1f, 0), hit.point, Color.green, 5f);
        //         transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
        //     }
        // }
    }

}
