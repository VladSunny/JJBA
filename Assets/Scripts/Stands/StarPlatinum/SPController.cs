using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

using JJBA.Audio;
using JJBA.VFX;
using JJBA.Stands.Movement;
using JJBA.Core;
using JJBA.UI;

namespace JJBA.Stands.StarPlatinum.Controller
{
    [RequireComponent(typeof(StandMover))]

    public class SPController : MonoBehaviour
    {
        [SerializeField] private float _summonCooldown = 1f;

        public bool _usingSkill = false;

        [SerializeField] private GameObject _standModel;
        private StandMover _mover;
        private AudioManager _audioManager;
        private ParticleManager _particleManager;
        private ToggleVisibility _toggleVisibility;
        private CooldownUIManager _cooldownUIManager;
        private GameObject _user;

        private bool _isActive;
        private float _summonTimer;
        private float _comboTimer = -1f;

        public void Initialize(GameObject user)
        {
            _user = user;

            _mover = GetComponent<StandMover>();
            _audioManager = GetComponentInChildren<AudioManager>();
            _particleManager = GetComponentInChildren<ParticleManager>();
            _toggleVisibility = GetComponentInChildren<ToggleVisibility>();
            _cooldownUIManager = _user.GetComponent<CooldownUIManager>();

            _summonTimer = _summonCooldown + 1f;

            SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            if (_summonTimer <= _summonCooldown) return;

            _isActive = isActive;

            if (!_isActive) Hide();
            else Summon();

            _summonTimer = 0f;

            if (_cooldownUIManager != null)
                _cooldownUIManager.AddCooldownTimer(_summonCooldown, "Summon");
        }

        public bool IsActive()
        {
            return _isActive;
        }

        private void Update()
        {
            if (_isActive)
                _standModel.SetActive(true);

            if (_summonTimer <= _summonCooldown)
                _summonTimer += Time.deltaTime;

            if (_comboTimer >= 0)
            {
                _comboTimer -= Time.deltaTime;
                if (_comboTimer <= 0 && _usingSkill == false)
                    _mover.Idle();
            }
        }

        private async void Hide()
        {
            _audioManager.Play("Hide");
            await _mover.Hide();
            _toggleVisibility.SetVisibility(false);
        }

        private void Summon()
        {
            _audioManager.Play("Summon" + Random.Range(1, 6));
            _particleManager.Play("Summon");
            _toggleVisibility.SetVisibility(true);
            _mover.Summon();
        }

        public void EndSkillWithComboTime(float time)
        {
            _comboTimer = time;
        }
    }
}
