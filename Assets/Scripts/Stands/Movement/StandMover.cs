using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JJBA.Stands.Movement
{
    enum MoveState
    {
        Idle,
        UsingSkill,
        Hide
    }

    public class StandMover : MonoBehaviour
    {
        [SerializeField] private float _followDuration = 0.3f;
        [SerializeField] private float _usingSkillDuration = 0.15f;
        [SerializeField] private float _followDistance = 0.01f;

        private Animator _animator;

        private Transform _target;
        private Transform _playerOrientation;
        private GameObject _user;
        private Transform _idlePosition;
        private Transform _usingSkillPosition;
        private MoveState _currentState = MoveState.Idle;

        public void Initialize(Transform playerOrientation, Transform idlePosition, Transform usingSkillPosition, GameObject user)
        {
            _playerOrientation = playerOrientation;
            _idlePosition = idlePosition;
            _usingSkillPosition = usingSkillPosition;
            _user = user;

            _animator = GetComponentInChildren<Animator>();

            Idle();
        }

        private void Update()
        {
            if (_target == null) return;

            if (Vector3.Distance(transform.position, _target.position) > _followDistance)
            {
                float duration;

                if (_currentState == MoveState.UsingSkill) duration = _usingSkillDuration;
                else duration = _followDuration;

                transform.DOMove(_target.position, duration);
            }

            if (_playerOrientation != null)
                transform.forward = _playerOrientation.forward;
        }

        public async UniTask Hide()
        {
            ChangeState(MoveState.Hide);
            await transform.DOMove(_playerOrientation.position, _followDuration).AsyncWaitForCompletion();
        }

        public void Idle()
        {
            ChangeState(MoveState.Idle);
        }

        public void Summon()
        {
            _animator.SetTrigger("Spawn");
            ChangeState(MoveState.Idle);
        }

        public void UsingSkill()
        {
            if (_currentState != MoveState.Idle) return;
            ChangeState(MoveState.UsingSkill);
        }

        private void ChangeState(MoveState state)
        {
            _currentState = state;

            if (_currentState == MoveState.Idle) _target = _idlePosition;
            if (_currentState == MoveState.Hide) _target = _playerOrientation;
            if (_currentState == MoveState.UsingSkill) _target = _usingSkillPosition;
        }
    }
}
