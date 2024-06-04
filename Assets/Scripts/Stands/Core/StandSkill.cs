using UnityEngine;
using System.Threading.Tasks;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.UI;
using JJBA.Combat;
using JJBA.Movement;
using JJBA.Core;

namespace JJBA.Stands.Core
{
    public abstract class StandSkill : MonoBehaviour
    {
        [SerializeField] private float _cooldown = 0.5f;

        [SerializeField] protected float _damage = 10f;
        [SerializeField] protected float _force = 0f;
        [SerializeField] protected Vector3 _hitBoxSize = new Vector3(1f, 1f, 1f);
        [SerializeField] protected float _forwardBoxOffset = 1f;

        protected string _skillName = "Skill";
        protected DamageType _damageType = DamageType.BASE;
        protected GameObject _user;

        private Mover _userMover;
        private PunchEvent _punchEvent;
        private CooldownUIManager _cooldownUIManager;
        private SPController _standController;
        private StandMover _mover;
        private DynamicHitBox _dynamicHitBox;

        private float _cooldownTimer = 0f;

        public virtual void Initialize(SPController standController, GameObject user)
        {
            _standController = standController;
            _user = user;

            _mover = GetComponent<StandMover>();
            _dynamicHitBox = GetComponent<DynamicHitBox>();
            _punchEvent = GetComponentInChildren<PunchEvent>();
            _cooldownUIManager = _user.GetComponent<CooldownUIManager>();
            _userMover = _user.GetComponent<Mover>();

            _punchEvent.punchEvent.AddListener(Punch);
        }

        protected virtual void Update()
        {
            if (_cooldownTimer > 0) _cooldownTimer -= Time.deltaTime;
        }

        public virtual bool Use()
        {
            if (!_standController.IsActive() || _cooldownTimer > 0) return false;

            _userMover.SetRunning(false);

            _cooldownTimer = _cooldown;

            _mover.UsingSkill();

            if (_cooldownUIManager != null)
                _cooldownUIManager.AddCooldownTimer(_cooldown, _skillName);

            return true;

            // string soundName = "FinisherPunch" + Random.Range(1, 3);
            // Debug.Log(soundName);
            // _audioManager.Play(soundName);

            // _animator.SetTrigger("FinisherPunch");
        }

        protected virtual async void Punch()
        {
            _dynamicHitBox.CreateHitBox(
                Vector3.forward * _forwardBoxOffset,
                _hitBoxSize,
                HitFunction,
                true
            );

            await Task.Delay((int)(0.5f * 1000));
            _mover.Idle();
        }

        protected virtual void HitFunction(Collider collider)
        {
            if (collider.transform == _user.transform || !(collider is CapsuleCollider capsuleCollider))
                return;

            Health enemyHealth = collider.transform.GetComponent<Health>();
            Damage damage;

            damage = new()
            {
                damageValue = _damage,
                from = transform.position,
                forse = transform.forward * _force,
                type = _damageType
            };

            if (enemyHealth != null)
                collider.transform.GetComponent<Health>().GetDamage(damage);
        }
    }
}
