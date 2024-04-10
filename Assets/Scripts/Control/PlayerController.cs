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

        [Header("Keybinds")]
        public KeyCode jumpKey = KeyCode.Space;
        
        private PlayerInput _playerInput;
        private Animator _animator;
        private Mover _mover;
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _mover = GetComponent<Mover>();
            
            _playerInput = GetComponent<PlayerInput>();
            
            _moveAction = _playerInput.actions["Movement"];
            _lookAction = _playerInput.actions["Look"];
            _jumpAction = _playerInput.actions["Jump"];
        }

        private void Update()
        {
            MyInput();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MyInput()
        {
            if (Input.GetKey(jumpKey) && _readyToJump)
            {
                if (_mover != null)
                {
                    _mover.Jump();
                    _readyToJump = false;
                    Invoke(nameof(ResetJump), jumpCooldown);
                }
            }
        }

        private void MovePlayer()
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            if (_mover) _mover.MovePlayer(input.y, input.x);
            
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