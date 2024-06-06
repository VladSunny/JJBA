using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Combat;
using JJBA.Audio;
using JJBA.Stands.Core;
using JJBA.UI;
using JJBA.VFX;
using JJBA.Movement;
using JJBA.Stands.Movement;
using JJBA.Core;

namespace JJBA
{
    public class BarrageSkill : MonoBehaviour
    {
        [SerializeField] private float _cooldown = 3f;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private float _punchTime = 0.2f;
        [SerializeField] private float _force = 2f;

        private ParticleManager _particleManager;
        private AudioManager _audioManager;
        private CooldownUIManager _cooldownUIManager;
        private StandMover _mover;
        private string _skillName = "Barrage";
        private float _cooldownTimer = 0f;
        private float _barrageTimer = 0f;
        private float _punchTimer = 0f;
        private bool _inProcess = false;
        private SPController _standController;
        private GameObject _user;
        private DynamicHitBox _dynamicHitBox;
        private Animator _animator;

        public void Initialize(SPController standController, GameObject user)
        {
            _standController = standController;
            _user = user;

            _particleManager = GetComponentInChildren<ParticleManager>();
            _mover = GetComponent<StandMover>();
            _cooldownUIManager = _user.GetComponent<CooldownUIManager>();
            _dynamicHitBox = GetComponent<DynamicHitBox>();
            _animator = GetComponentInChildren<Animator>();
            _audioManager = GetComponentInChildren<AudioManager>();
        }

        protected void Update()
        {
            if (_cooldownTimer > 0) _cooldownTimer -= Time.deltaTime;
            if (_barrageTimer > 0) _barrageTimer -= Time.deltaTime;

            if (_barrageTimer <= 0 && _inProcess) Stop();
            if (_punchTimer > 0) _punchTimer -= Time.deltaTime;

            if (_punchTimer <= 0 && _inProcess)
            {
                _punchTimer = _punchTime;
                Punch();
            }
        }

        protected virtual bool CantUseSkill()
        {
            return !_standController.IsActive() || _cooldownTimer > 0 || _standController._usingSkill || _inProcess;
        }

        public bool Use()
        {
            if (CantUseSkill()) return false;

            _particleManager.Play(_skillName);
            _mover.UsingSkill();

            _inProcess = true;
            _barrageTimer = _duration;

            _standController._usingSkill = true;

            _punchTimer = _punchTime;

            _animator.SetBool("Barraging", true);
            _audioManager.Play("Barrage");

            return true;
        }

        public void Stop()
        {
            if (!_inProcess) return;

            _particleManager.Stop(_skillName);
            _standController.EndSkillWithComboTime(1f);
            _cooldownTimer = _cooldown;
            _inProcess = false;
            _cooldownUIManager.AddCooldownTimer(_cooldownTimer, _skillName);
            _standController._usingSkill = false;

            _animator.SetBool("Barraging", false);
            _audioManager.Stop("Barrage");
        }

        protected void Punch()
        {
            _dynamicHitBox.CreateHitBox(
                Vector3.forward * 0.5f,
                new Vector3(1f, 1f, 2f),
                HitFunction,
                true
            );
        }

        protected void HitFunction(Collider collider)
        {
            if (collider.transform == _user.transform || !(collider is CapsuleCollider capsuleCollider))
                return;

            Health enemyHealth = collider.transform.GetComponent<Health>();
            Damage damage;

            damage = new()
            {
                damageValue = 5f,
                from = transform.position,
                forse = transform.forward * _force,
                type = DamageType.BASE
            };

            if (enemyHealth != null)
                collider.transform.GetComponent<Health>().GetDamage(damage);
        }
    }
}
