using System;
using System.Collections;
using System.Collections.Generic;
using JJBA.Core;
using UnityEngine;

namespace JJBA.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class Mover : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float groundDrag = 5f;
        [SerializeField] private float turnSpeed = 5f;
        [SerializeField] private float runMultiplier = 2f;
        [SerializeField] private float smoothingFactor = 0.5f;

        [Header("Jump")]
        [SerializeField] private float jumpCooldown = 0.25f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float airMultiplier = 0.4f;
        private bool _readyToJump = true;

        [Header("Dependencies")]
        public Transform orientation;
        [SerializeField] private GroundCheck groundCheck;

        // AV - animator value
        private static readonly int fallingAV = Animator.StringToHash("falling");
        private static readonly int jumpAV = Animator.StringToHash("jump");
        private static readonly int turnAV = Animator.StringToHash("turn");
        private static readonly int walkSpeedAV = Animator.StringToHash("walkSpeed");
        private static readonly int walkAV = Animator.StringToHash("walk");
        private static readonly int runAV = Animator.StringToHash("isRunning");

        private Vector3 _moveDirection;
        private Rigidbody _rb;
        private Animator _animator;

        private float _turn;
        private float _lastHorizontal;
        private bool _isRunning = false;
        private Vector3 moveForce;
        private float _runningState = 0f;

        public void SetRunning(bool running)
        {
            _isRunning = running;
        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        public void Initialize()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            if (groundCheck.IsGrounded())
            {
                _rb.drag = groundDrag;
                _animator.SetBool(fallingAV, false);
            }
            else
            {
                _rb.drag = 0;
                _animator.SetBool(fallingAV, true);
            }

            _animator.SetFloat(runAV, _runningState);

            _turn = Mathf.Lerp(_turn, _lastHorizontal, turnSpeed * Time.deltaTime);

            if (_isRunning)
                _runningState = Mathf.Lerp(_runningState, 1f, smoothingFactor * Time.deltaTime);
            else
                _runningState = Mathf.Lerp(_runningState, 0f, smoothingFactor * Time.deltaTime);

            SpeedControl();
        }

        public void MovePlayer(float vertical, float horizontal)
        {
            _lastHorizontal = horizontal;
            _moveDirection = orientation.forward * vertical + orientation.right * horizontal;

            _animator.SetFloat(turnAV, _turn);

            moveForce = _moveDirection * (moveSpeed * 10f);

            if (groundCheck.IsGrounded())
            {
                if (_isRunning) moveForce *= runMultiplier;
                _rb.AddForce(moveForce, ForceMode.Force);

                if (vertical < -0.01)
                    _animator.SetFloat(walkSpeedAV, -1);
                else
                    _animator.SetFloat(walkSpeedAV, 1);

                if (Math.Abs(vertical) > 0.01 || Math.Abs(horizontal) > 0.01)
                    _animator.SetBool(walkAV, true);
                else
                    _animator.SetBool(walkAV, false);
            }

            else if (!groundCheck.IsGrounded())
                _rb.AddForce(moveForce * airMultiplier, ForceMode.Force);
        }

        public void Jump()
        {
            if (!_readyToJump) return;

            if (groundCheck.IsGrounded())
            {
                _animator.SetTrigger(jumpAV);
                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                _readyToJump = false;
                Invoke(nameof(ResetJump), jumpCooldown);
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

        private void ResetJump()
        {
            _readyToJump = true;
        }
    }
}