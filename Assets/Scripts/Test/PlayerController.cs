using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    private PlayerInput _playerInput;
    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        
        _moveAction = _playerInput.actions["Movement"];
        _lookAction = _playerInput.actions["Look"];
        _jumpAction = _playerInput.actions["Jump"];
    }

    void Update()
    {
        if (_controller.isGrounded)
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            
            _moveDirection = new Vector3(input.x, 0.0f, input.y);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= speed;
            
            if (_jumpAction.triggered)
            {
                _moveDirection.y = jumpSpeed;
            }
        }

        _moveDirection.y -= gravity * Time.deltaTime;
        
        _controller.Move(_moveDirection * Time.deltaTime);
    }
}
