using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JJBA.Stands.StarPlatinum.Skill;
using JJBA.Stands.StarPlatinum.Controller;
namespace JJBA.Stands.StarPlatinum.Input
{
    public class SPInput : MonoBehaviour
    {
        private SPController _standController;
        private PlayerInput _playerInput;
        private BasePunchSkill _basePunchSkill;
        private GameObject _stand;

        private InputAction _summonAction;
        private InputAction _basePunchAction;

        public void Initialize(SPController standController, GameObject stand)
        {
            _standController = standController;
            _stand = stand;

            _playerInput = GetComponent<PlayerInput>();
            _basePunchSkill = _stand.GetComponent<BasePunchSkill>();

            _summonAction = _playerInput.actions["Summon"];
            _basePunchAction = _playerInput.actions["BasePunch"];
        }

        private void Update()
        {
            MyInput();
        }

        private void MyInput()
        {
            if (_summonAction.triggered)
                _standController.SetActive(!_standController.IsActive());
            if (_basePunchAction.triggered)
                _basePunchSkill.Use();
        }
    }
}
