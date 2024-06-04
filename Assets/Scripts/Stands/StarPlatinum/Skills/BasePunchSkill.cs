using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Combat;
using JJBA.Audio;
using JJBA.Stands.Core;

namespace JJBA.Stands.StarPlatinum.Skill
{
    public class BasePunchSkill : StandSkill
    {
        private Animator _animator;

        public override void Initialize(SPController standController, GameObject user)
        {
            base.Initialize(standController, user);

            _animator = GetComponentInChildren<Animator>();
            _skillName = "Base Punch";
            _damageType = DamageType.BASE;
        }

        public override bool Use()
        {
            if (!base.Use()) return false;

            _animator.SetTrigger("BasePunch");

            return true;
        }

        protected override void Punch()
        {
            base.Punch();
        }
    }
}
