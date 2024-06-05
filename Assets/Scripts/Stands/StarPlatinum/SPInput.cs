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
        private FinisherPunchSkill _finisherPunchSkill;
        private BasePunchSkill _basePunchSkill;
        private BarrageSkill _barrageSkill;
        private GameObject _stand;

        private InputAction _summonAction;
        private InputAction _finisherPunchAction;
        private InputAction _basePunchAction;
        private InputAction _barrageAction;

        public void Initialize(SPController standController, GameObject stand)
        {
            _standController = standController;
            _stand = stand;

            _playerInput = GetComponent<PlayerInput>();

            _finisherPunchSkill = _stand.GetComponent<FinisherPunchSkill>();
            _basePunchSkill = _stand.GetComponent<BasePunchSkill>();
            _barrageSkill = _stand.GetComponent<BarrageSkill>();

            _summonAction = _playerInput.actions["Summon"];
            _finisherPunchAction = _playerInput.actions["FinisherPunch"];
            _basePunchAction = _playerInput.actions["BasePunch"];
            _barrageAction = _playerInput.actions["Barrage"];
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
                _finisherPunchSkill.Use();

            if (_basePunchAction.triggered)
                _basePunchSkill.Use();

            if (_barrageAction.triggered)
                _barrageSkill.Use();

            _barrageAction.performed += OnBarragePerformed;
            _barrageAction.canceled += OnBarrageCanceled;

            // if ((int)_barrageAction.ReadValue<float>() == 1)
            // {
            //     _barrageSkill.Use();
            //     Debug.Log("barrage");
            // }
            // else if ((int)_barrageAction.ReadValue<float>() == 0)
            //     _barrageSkill.Stop();

        }

        private void OnBarragePerformed(InputAction.CallbackContext context)
        {
            _barrageSkill.Use();
        }

        private void OnBarrageCanceled(InputAction.CallbackContext context)
        {
            _barrageSkill.Stop();
        }
    }
}
