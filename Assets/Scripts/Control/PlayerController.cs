using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Movement;
using JJBA.Combat;
using UnityEngine.InputSystem;

namespace JJBA.Control
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent (typeof(CharacterFighter))]
    [RequireComponent (typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        
        private PlayerInput _playerInput;
        private Mover _mover;
        private CharacterFighter _fighter;
        
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _basePunchAction;
        private Vector2 _input;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<CharacterFighter>();
            
            _playerInput = GetComponent<PlayerInput>();
            
            _moveAction = _playerInput.actions["Movement"];
            _jumpAction = _playerInput.actions["Jump"];
            _basePunchAction = _playerInput.actions["BasePunch"];
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
            if (_jumpAction.triggered)
            {
                _mover.Jump();
            }
            if (_basePunchAction.triggered)
            {
                _fighter.BasePunch();
            }
        }

        private void MovePlayer()
        {
            _input = _moveAction.ReadValue<Vector2>();
            _mover.MovePlayer(_input.y, _input.x);
        }
    }
}