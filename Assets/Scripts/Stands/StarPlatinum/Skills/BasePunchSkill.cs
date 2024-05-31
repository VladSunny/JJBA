using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.UI;
using System.Threading.Tasks;

namespace JJBA.Stands.StarPlatinum.Skill
{
    public class BasePunchSkill : MonoBehaviour
    {
        [SerializeField] private float _cooldown = 0.5f;
        private StandMover _mover;
        private CooldownUIManager _cooldownUIManager;
        private SPController _standController;

        public void Initialize(CooldownUIManager cooldownUIManager, SPController standController)
        {
            _mover = GetComponent<StandMover>();

            _standController = standController;
            _cooldownUIManager = cooldownUIManager;
        }

        public async void Use()
        {
            if (!_standController.IsActive()) return;

            _mover.UsingSkill();

            if (_cooldownUIManager != null)
                _cooldownUIManager.AddCooldownTimer(_cooldown, "Star Punch");

            await Task.Delay((int)(_cooldown * 1000));

            _mover.Idle();
        }
    }
}
