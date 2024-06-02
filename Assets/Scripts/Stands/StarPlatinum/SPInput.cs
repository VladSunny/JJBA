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
        private FinisherPunchSkill _basePunchSkill;
        private GameObject _stand;

        private InputAction _summonAction;
        private InputAction _finisherPunchAction;

        public void Initialize(SPController standController, GameObject stand)
        {
            _standController = standController;
            _stand = stand;

            _playerInput = GetComponent<PlayerInput>();
            _basePunchSkill = _stand.GetComponent<FinisherPunchSkill>();

            _summonAction = _playerInput.actions["Summon"];
            _finisherPunchAction = _playerInput.actions["FinisherPunch"];
        }

        private void Update()
        {
            MyInput();
        }

        private void MyInput()
        {
            if (_summonAction.triggered)
                _standController.SetActive(!_standController.IsActive());
            if (_finisherPunchAction.triggered)
                _basePunchSkill.Use();
        }
    }
}
