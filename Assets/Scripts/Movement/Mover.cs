using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
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

        [Header("Ground Check")]
        [SerializeField] private float playerHeight = 2f;
        [SerializeField] private LayerMask whatIsGround;
        private bool _grounded;
        private bool _boxGrounded;

        [Header("Dependencies")]
        public Transform orientation;

        // AV - animator value
        private static readonly int FallingAV = Animator.StringToHash("falling");
        private static readonly int JumpAV = Animator.StringToHash("jump");
        private static readonly int TurnAV = Animator.StringToHash("turn");
        private static readonly int WalkSpeedAV = Animator.StringToHash("walkSpeed");
        private static readonly int WalkAV = Animator.StringToHash("walk");
        private static readonly int RunAV = Animator.StringToHash("isRunning");

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

        public bool IsGrounded()
        {
            return _grounded;
        }

        public void Initialize()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _rb.freezeRotation = true;
        }

        void Start()
        {
            // Initialize();
        }

        private void Update()
        {
            _grounded = CheckGround() || _boxGrounded;

            if (_grounded)
            {
                _rb.drag = groundDrag;
                _animator.SetBool(FallingAV, false);
            }
            else
            {
                _rb.drag = 0;
                _animator.SetBool(FallingAV, true);
            }

            _animator.SetFloat(RunAV, _runningState);

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

            _animator.SetFloat(TurnAV, _turn);

            moveForce = _moveDirection * (moveSpeed * 10f);

            if (_grounded)
            {
                if (_isRunning) moveForce *= runMultiplier;
                _rb.AddForce(moveForce, ForceMode.Force);

                if (vertical < -0.01)
                    _animator.SetFloat(WalkSpeedAV, -1);
                else
                    _animator.SetFloat(WalkSpeedAV, 1);

                if (Math.Abs(vertical) > 0.01 || Math.Abs(horizontal) > 0.01)
                    _animator.SetBool(WalkAV, true);
                else
                    _animator.SetBool(WalkAV, false);
            }

            else if (!_grounded)
                _rb.AddForce(moveForce * airMultiplier, ForceMode.Force);
        }

        public void Jump()
        {
            if (!_readyToJump) return;

            if (_grounded)
            {
                _animator.SetTrigger(JumpAV);
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
    }
}