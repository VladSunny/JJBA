using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.UI;
using System.Threading.Tasks;

namespace JJBA.Stands.StarPlatinum.Skill
{
    public class FinisherPunchSkill : MonoBehaviour
    {
        [SerializeField] private float _cooldown = 0.5f;
        private float _cooldownTimer = 0f;
        private StandMover _mover;
        private CooldownUIManager _cooldownUIManager;
        private SPController _standController;

        public void Initialize(CooldownUIManager cooldownUIManager, SPController standController)
        {
            _mover = GetComponent<StandMover>();

            _standController = standController;
            _cooldownUIManager = cooldownUIManager;
        }

        private void Update() {
            if (_cooldownTimer > 0) _cooldownTimer -= Time.deltaTime;
        }

        public async void Use()
        {
            if (!_standController.IsActive() || _cooldownTimer > 0) return;

            _cooldownTimer = _cooldown;

            _mover.UsingSkill();

            if (_cooldownUIManager != null)
                _cooldownUIManager.AddCooldownTimer(_cooldown, "Star Finisher Punch");

            await Task.Delay((int)(2f * 1000));

            _mover.Idle();
        }
    }
}
