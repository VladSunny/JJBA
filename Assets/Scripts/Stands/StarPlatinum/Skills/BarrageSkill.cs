using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Combat;
using JJBA.Audio;
using JJBA.Stands.Core;
using JJBA.UI;
using JJBA.VFX;

namespace JJBA
{
    public class BarrageSkill : StandSkill
    {
        private ParticleManager _particleManager;
        private bool _isPunching = false;

        public override void Initialize(SPController standController, GameObject user)
        {
            base.Initialize(standController, user);

            _particleManager = GetComponentInChildren<ParticleManager>();

            _skillName = "Barrage";
        }

        protected override void Update()
        {

        }

        public override bool Use()
        {
            _particleManager.Play(_skillName);

            return true;
        }

        public void Stop()
        {
            _particleManager.Stop(_skillName);
        }

        protected override void Punch()
        {
        }
    }
}
