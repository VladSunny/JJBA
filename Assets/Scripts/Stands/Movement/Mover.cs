using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

using JJBA.Stands.StarPlatinum.Controller;
using Cysharp.Threading.Tasks;
namespace JJBA.Stands.Movement
{
    public class Mover : MonoBehaviour
    {
        private Transform _target;
        private Transform _playerOrientation;
        private Transform _idlePosition;
        private SPController _standController;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float followDistance = 0.01f;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Initialize(Transform playerOrientation, Transform idlePosition)
        {
            _playerOrientation = playerOrientation;
            _idlePosition = idlePosition;
            _standController = GetComponentInParent<SPController>();
        }

        private void Update()
        {
            if (_target != null && Vector3.Distance(transform.position, _target.position) > followDistance)
            {
                transform.DOMove(_target.position, duration);
            }

            if (_playerOrientation != null)
                transform.forward = _playerOrientation.forward;

            if (_standController != null) {
                if (_standController.IsActive()) _target = _idlePosition;
                else _target = _playerOrientation;
            }
        }

        public async UniTask Hide() {
            await transform.DOMove(_playerOrientation.position, duration).AsyncWaitForCompletion();
        }

    }
}
