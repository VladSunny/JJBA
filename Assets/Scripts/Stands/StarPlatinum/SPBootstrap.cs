using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Stands.StarPlatinum.Skill;
using JJBA.UI;

namespace JJBA.Stands.StarPlatinum.Bootstrap
{
    [RequireComponent(typeof(StandMover))]
    [RequireComponent(typeof(SPController))]
    [RequireComponent(typeof(FinisherPunchSkill))]
    public class SPBootstrap : MonoBehaviour
    {
        private Transform _playerOrientation;
        private Transform _idlePosition;
        private Transform _skillPosition;
        private GameObject _user;

        public void Initialize(Transform playerOrientation, Transform idlePosition, Transform skillPosition, GameObject user)
        {
            _playerOrientation = playerOrientation;
            _idlePosition = idlePosition;
            _skillPosition = skillPosition;
            _user = user;

            GetComponent<StandMover>().Initialize(
                _playerOrientation,
                _idlePosition,
                _skillPosition,
                _user);

            GetComponent<SPController>().Initialize(_user);

            GetComponent<FinisherPunchSkill>().Initialize(
                GetComponent<SPController>(),
                _user
            );
            GetComponent<BasePunchSkill>().Initialize(
                GetComponent<SPController>(),
                _user
            );
        }
    }
}
