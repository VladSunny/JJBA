using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Combat;
using JJBA.Audio;
using JJBA.Stands.Core;
using JJBA.UI;

namespace JJBA.Stands.StarPlatinum.Skill
{
    public class BasePunchSkill : PunchStandSkill
    {
        [SerializeField] private int _maxPunches = 5;
        [SerializeField] private float _comboCooldown = 2f;
        [SerializeField] private float _timeForCombo = 1f;
        [SerializeField] private float _finisherFroce = 10f;

        private Animator _animator;
        private CooldownUIManager _cooldownUIManager;

        private int _punchCounter = 0;
        private float _punchCooldown;
        private float _baseForce;
        private float _comboTimer = -1f;


        public override void Initialize(SPController standController, GameObject user)
        {
            base.Initialize(standController, user);

            _animator = GetComponentInChildren<Animator>();
            _cooldownUIManager = _user.GetComponent<CooldownUIManager>();

            _skillName = "Base Punch";
            _damageType = DamageType.BASE;
            _punchCooldown = _cooldown;
            _baseForce = _force;
        }

        protected override void Update()
        {
            base.Update();
            if (_comboTimer >= 0f && _cooldownTimer <= 0f)
            {
                _comboTimer -= Time.deltaTime;
                if (_comboTimer <= 0f)
                {
                    _punchCounter = 0;
                }
            }
        }

        public override bool Use()
        {
            if (CantUseSkill()) return false;

            _comboTimer = _timeForCombo;
            _punchCounter++;
            _animator.SetTrigger("BasePunch" + (_punchCounter % 2 + 1));

            if (_punchCounter >= _maxPunches)
            {
                _punchCounter = 0;
                _cooldown = _comboCooldown;
                _damageType = DamageType.PUNCH_FINISHER;
                _force = _finisherFroce;
            }
            else
            {
                _cooldown = _punchCooldown;
                _force = _baseForce;
                _damageType = DamageType.BASE;
            }

            base.Use();

            return true;
        }

        protected override void Punch()
        {
            if (!_usingSkill) return;

            if (_cooldownUIManager != null && _cooldown == _comboCooldown)
                _cooldownUIManager.AddCooldownTimer(_cooldownTimer, _skillName);

            base.Punch();
        }
    }
}
