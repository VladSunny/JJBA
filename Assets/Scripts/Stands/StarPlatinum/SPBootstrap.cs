using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Stands.StarPlatinum.Input;
using JJBA.Stands.StarPlatinum.Skill;

namespace JJBA.Stands.StarPlatinum.Bootstrap
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(SPController))]
    [RequireComponent(typeof(BasePunchSkill))]
    public class SPBootstrap : MonoBehaviour
    {
        private Transform _playerOrientation;
        private Transform _idlePosition;
        private Transform _skillPosition;

        public void Initialize(Transform playerOrientation, Transform idlePosition, Transform skillPosition)
        {
            _playerOrientation = playerOrientation;
            _idlePosition = idlePosition;
            _skillPosition = skillPosition;

            GetComponent<Mover>().Initialize(_playerOrientation, _idlePosition);
            GetComponent<SPController>().Initialize();
            GetComponent<BasePunchSkill>().Initialize(_skillPosition);
        }
    }
}
