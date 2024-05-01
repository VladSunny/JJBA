using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Movement;
namespace JJBA.Combat
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Mover))]

    public class FallDamage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float safeFallDistance = 5f;
        [SerializeField] private float maxFallDistance = 20f;
        [SerializeField] private int maxDamage = 50;

        private Vector3 _lastGroundedPosition;
        private bool _isFalling = false;
        private Health _healthComponent;
        private Mover _mover;

        void Start()
        {
            _healthComponent = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _lastGroundedPosition = transform.position;
        }

        void Update()
        {

            if (_mover.IsGrounded())
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
    }
}
