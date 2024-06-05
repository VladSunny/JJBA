using UnityEngine;
using System.Threading.Tasks;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Combat;
using JJBA.Movement;
using JJBA.Core;

namespace JJBA.Stands.Core
{
    public abstract class StandSkill : MonoBehaviour
    {
        [SerializeField] protected float _cooldown = 0.5f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] protected float _force = 0f;
        [SerializeField] private Vector3 _hitBoxSize = new Vector3(1f, 1f, 1f);
        [SerializeField] private float _forwardBoxOffset = 1f;
        [SerializeField] private float _afterSkillDelay = 0.5f;

        protected string _skillName = "Skill";
        protected DamageType _damageType = DamageType.BASE;
        protected GameObject _user;

        private Mover _userMover;
        private PunchEvent _punchEvent;
        private SPController _standController;
        private StandMover _mover;
        private DynamicHitBox _dynamicHitBox;

        protected float _cooldownTimer = 0f;
        protected bool _usingSkill = false;

        public virtual void Initialize(SPController standController, GameObject user)
        {
            _standController = standController;
            _user = user;

            _mover = GetComponent<StandMover>();
            _dynamicHitBox = GetComponent<DynamicHitBox>();
            _punchEvent = GetComponentInChildren<PunchEvent>();
            _userMover = _user.GetComponent<Mover>();

            _punchEvent.punchEvent.AddListener(Punch);
        }

        protected virtual void Update()
        {
            if (_cooldownTimer > 0) _cooldownTimer -= Time.deltaTime;
        }

        protected virtual bool CantUseSkill()
        {
            return !_standController.IsActive() || _cooldownTimer > 0 || _standController._usingSkill;
        }

        public virtual bool Use()
        {
            if (CantUseSkill())
                return false;

            _userMover.SetRunning(false);

            _cooldownTimer = _cooldown;

            _mover.UsingSkill();

            _usingSkill = true;
            _standController._usingSkill = true;

            return true;
        }

        protected virtual void Punch()
        {
            if (!_usingSkill) return;

            _dynamicHitBox.CreateHitBox(
                Vector3.forward * _forwardBoxOffset,
                _hitBoxSize,
                HitFunction,
                true
            );

            _standController.EndSkillWithComboTime(_afterSkillDelay);
            _usingSkill = false;
            _standController._usingSkill = false;
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
