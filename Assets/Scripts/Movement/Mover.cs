using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Movement
{
    public class Mover : MonoBehaviour
    {
        [Header("Movement")] public float moveSpeed = 7f;

        public float groundDrag = 5f;

        public float jumpForce = 12f;
        public float airMultiplier = 0.4f;

        [Header("Ground Check")] public float playerHeight = 2f;
        public LayerMask whatIsGround;

        private bool _grounded;

        [Header("Dependencies")] public Transform orientation;

        private Vector3 _moveDirection;
        private Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            SpeedControl();
        }

        public void MovePlayer(float vertical, float horizontal)
        {
            _moveDirection = orientation.forward * vertical + orientation.right * horizontal;

            if (_grounded)
            {
                _rb.AddForce(_moveDirection * (moveSpeed * 10f), ForceMode.Force);
            }

            else if (!_grounded)
                _rb.AddForce(_moveDirection * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }

        private void Jump()
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}