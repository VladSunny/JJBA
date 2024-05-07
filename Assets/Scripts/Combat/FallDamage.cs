using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Movement;
using JJBA.Core;
using JJBA.Ragdoll;
namespace JJBA.Combat
{
    [RequireComponent(typeof(Health))]

    public class FallDamage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float safeFallDistance = 5f;
        [SerializeField] private float maxFallDistance = 20f;
        [SerializeField] private int maxDamage = 50;
        [SerializeField] private float minFallSpeed = 10f;
        [SerializeField] private float damageMultiplier = 1.0f;

        [Header("Dependencies")]
        [SerializeField] private LayerMask ground;
        [SerializeField] private GroundCheck groundCheck;
        [SerializeField] private Transform hipsBone;

        private Vector3 _lastGroundedPosition;
        private bool _isFalling = false;
        private Health _healthComponent;
        private RagdollSystem _ragdollSystem;
        private GroundCheck _hipsBoneGroundCheck;

        public void Initialize()
        {
            _healthComponent = GetComponent<Health>();
            _ragdollSystem = GetComponentInParent<RagdollSystem>();
            _hipsBoneGroundCheck = hipsBone.GetComponent<GroundCheck>();

            _lastGroundedPosition = transform.position;
            _hipsBoneGroundCheck.onCollision.AddListener(OnHipsCollision);
        }

        void Update()
        {
            if (!_ragdollSystem.IsFall()) NotRagdollFallDamage();
        }

        private void NotRagdollFallDamage()
        {
            if (groundCheck.IsGrounded())
            {
                if (_isFalling)
                {

                    float fallDistance = _lastGroundedPosition.y - transform.position.y;
                    if (fallDistance > safeFallDistance)
                    {
                        float damagePercent = Mathf.Clamp01((fallDistance - safeFallDistance) / (maxFallDistance - safeFallDistance));
                        int damage = Mathf.RoundToInt(damagePercent * maxDamage);
                        _healthComponent.Damage(damage);
                    }

                    _isFalling = false;
                }


                _lastGroundedPosition = transform.position;
            }
            else
            {
                _isFalling = true;
            }
        }


        private void OnHipsCollision(Collider other)
        {
            if (!_ragdollSystem.IsFall()) return;

            if (((1 << other.gameObject.layer) & ground) != 0)
            {
                Rigidbody hipsBoneRb = hipsBone.GetComponent<Rigidbody>();
                float fallSpeed = hipsBoneRb.velocity.magnitude;

                Debug.Log("fall speed: " + fallSpeed);

                if (fallSpeed > minFallSpeed)
                {
                    float damage = (fallSpeed - minFallSpeed) * damageMultiplier;
                    _healthComponent.Damage(damage);
                }
            }
        }
    }
}
