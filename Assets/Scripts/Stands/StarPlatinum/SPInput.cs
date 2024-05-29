using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JJBA.Stands.StarPlatinum.Controller;
namespace JJBA.Stands.StarPlatinum.Input
{
    public class SPInput : MonoBehaviour
    {
        private SPController _standController;
        private PlayerInput _playerInput;

        private InputAction _summonAction;

        public void Initialize(SPController standController)
        {
            _standController = standController;

            _playerInput = GetComponent<PlayerInput>();

            _summonAction = _playerInput.actions["Summon"];
        }

        private void Update()
        {
            MyInput();
        }

        private void MyInput()
        {
            if (_summonAction.triggered)
            {
                _standController.SetActive(!_standController.IsActive());
            }
        }
    }
}
