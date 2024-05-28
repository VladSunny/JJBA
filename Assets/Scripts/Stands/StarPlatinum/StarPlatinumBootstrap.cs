using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;

namespace JJBA.Stands.StarPlatinum.Bootstrap
{
    [RequireComponent(typeof(Mover))]
    public class StarPlatinumBootstrap : MonoBehaviour
    {
        private Transform _playerOrientation;
        private Transform _idlePosition;

        public void Initialize(Transform playerOrientation, Transform idlePosition)
        {
            _playerOrientation = playerOrientation;
            _idlePosition = idlePosition;

            GetComponent<Mover>().Initialize(_playerOrientation);
            GetComponent<Mover>().SetTarget(_idlePosition);
        }
    }
}
