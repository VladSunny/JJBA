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
    [RequireComponent(typeof(StandlessFighter))]
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {

        private PlayerInput _playerInput;
        private Mover _mover;
        private StandlessFighter _fighter;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _basePunchAction;
        private InputAction _runningAction;
        private Vector2 _input;

        public void Initialize()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<StandlessFighter>();
            _playerInput = GetComponent<PlayerInput>();

            _moveAction = _playerInput.actions["Movement"];
            _jumpAction = _playerInput.actions["Jump"];
            _basePunchAction = _playerInput.actions["BasePunch"];
            _runningAction = _playerInput.actions["Run"];
        }
        
        private void Start()
        {
            Initialize();
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
            if (_runningAction.triggered)
            {
                _mover.SetRunning(!_mover.IsRunning());
            }
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