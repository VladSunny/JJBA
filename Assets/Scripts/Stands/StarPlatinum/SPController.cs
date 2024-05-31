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

        [SerializeField] private GameObject _standModel;
        private StandMover _mover;
        private AudioManager _audioManager;
        private ParticleManager _particleManager;
        private ToggleVisibility _toggleVisibility;
        private CooldownUIManager _cooldownUIManager;

        private bool _isActive;
        private float _summonTimer;

        public void Initialize(CooldownUIManager cooldownUIManager)
        {
            _mover = GetComponent<StandMover>();
            _audioManager = GetComponentInChildren<AudioManager>();
            _particleManager = GetComponentInChildren<ParticleManager>();
            _toggleVisibility = GetComponentInChildren<ToggleVisibility>();

            _cooldownUIManager = cooldownUIManager;

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
        }

        private async void Hide()
        {
            _audioManager.Play("Hide");
            await _mover.Hide();
            _toggleVisibility.SetVisibility(false);
        }

        private void Summon()
        {
            _audioManager.Play("Summon" + Random.Range(1, 4));
            _particleManager.Play("Summon");
            _toggleVisibility.SetVisibility(true);
            _mover.Summon();
        }
    }
}
