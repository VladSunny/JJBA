using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Movement;
using UnityEngine.InputSystem;

namespace JJBA.Control
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Jump")]
        public float jumpCooldown = 0.25f;
        private bool _readyToJump = true;
        
        private PlayerInput _playerInput;
        private Animator _animator;
        private Mover _mover;
        
        private InputAction _moveAction;
        private InputAction _jumpAction;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _mover = GetComponent<Mover>();
            
            _playerInput = GetComponent<PlayerInput>();
            
            _moveAction = _playerInput.actions["Movement"];
            _jumpAction = _playerInput.actions["Jump"];
        }

        private void Update()
        {
            MyInput();

            if (!_mover.isGrounded())
                _animator.SetBool("falling", true);
            else 
                _animator.SetBool("falling", false);
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MyInput()
        {
            if (_jumpAction.triggered && _readyToJump)
            {
                if (_mover != null)
                {
                    _mover.Jump();
                    _readyToJump = false;
                    Invoke(nameof(ResetJump), jumpCooldown);
                    _animator.SetTrigger("jump");
                }
            }
        }

        private void MovePlayer()
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            if (_mover) _mover.MovePlayer(input.y, input.x);
            
            _animator.SetFloat("turn", input.x);
            
            if (input.y < -0.01)
                _animator.SetFloat("walkSpeed", -1);
            else
                _animator.SetFloat("walkSpeed", 1);

            if (Math.Abs(input.y) > 0.01 || Math.Abs(input.x) > 0.01)
                _animator.SetBool("walk", true);
            else
                _animator.SetBool("walk", false);
        }

        private void ResetJump()
        {
            _readyToJump = true;
        }
    }
}