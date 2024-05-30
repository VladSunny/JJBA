using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Stands.StarPlatinum.Input;

namespace JJBA.Stands.StarPlatinum.Bootstrap
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(SPController))]
    public class SPBootstrap : MonoBehaviour
    {
        private Transform _playerOrientation;
        private Transform _idlePosition;

        public void Initialize(Transform playerOrientation, Transform idlePosition)
        {
            _playerOrientation = playerOrientation;
            _idlePosition = idlePosition;

            GetComponent<Mover>().Initialize(_playerOrientation, _idlePosition);
            GetComponent<SPController>().Initialize();
        }
    }
}