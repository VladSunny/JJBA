using UnityEngine;

using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Combat;
using JJBA.Audio;
using JJBA.Stands.Core;
using JJBA.UI;

namespace JJBA.Stands.StarPlatinum.Skill
{
    public class FinisherPunchSkill : StandSkill
    {
        private Animator _animator;
        private AudioManager _audioManager;
        private CooldownUIManager _cooldownUIManager;

        public override void Initialize(SPController standController, GameObject user)
        {
            base.Initialize(standController, user);

            _audioManager = GetComponentInChildren<AudioManager>();
            _animator = GetComponentInChildren<Animator>();
            _cooldownUIManager = _user.GetComponent<CooldownUIManager>();
            _skillName = "Star Finisher Punch";
            _damageType = DamageType.PUNCH_FINISHER;
        }

        public override bool Use()
        {
            if (!base.Use()) return false;

            string soundName = "FinisherPunch" + Random.Range(1, 3);
            _audioManager.Play(soundName);

            _animator.SetTrigger("FinisherPunch");

            return true;
        }

        protected override void Punch()
        {
            if (!_usingSkill) return;

            if (_cooldownUIManager != null)
                _cooldownUIManager.AddCooldownTimer(_cooldownTimer, _skillName);
                
            base.Punch();
        }
    }
}
