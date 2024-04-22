using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Mover : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 7f;
        public float groundDrag = 5f;
        public float jumpForce = 12f;
        public float airMultiplier = 0.4f;

        [Header("Ground Check")]
        public float playerHeight = 2f;
        public LayerMask whatIsGround;
        private bool _grounded;
        private bool _boxGrounded;

        [Header("Dependencies")]
        public Transform orientation;

        // AV - animator value
        /*private static readonly int FallingAV = Animator.StringToHash("falling");
        private static readonly int JumpAV = Animator.StringToHash("jump");
        private static readonly int TurnAV = Animator.StringToHash("turn");
        private static readonly int WalkSpeedAV = Animator.StringToHash("walkSpeed");
        private static readonly int WalkAV = Animator.StringToHash("walk");*/

        private Vector3 _moveDirection;
        private Rigidbody _rb;
        private Animator _animator;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();

            _rb.freezeRotation = true;
        }

        private void Update()
        {
            _grounded = CheckGround() || _boxGrounded;

            if (_grounded)
            {
                _rb.drag = groundDrag;
                // _animator.SetBool(FallingAV, false);
            }
            else
            {
                _rb.drag = 0;
                // _animator.SetBool(FallingAV, true);
            }
            SpeedControl();
        }

        private bool CheckGround()
        {
            return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & whatIsGround) != 0)
            {
                _boxGrounded = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & whatIsGround) != 0)
            {
                _boxGrounded = false;
            }
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
        
        public void Jump()
        {
            if (_grounded)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
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

        public bool isGrounded()
        {
            return _grounded;
        }
    }
}