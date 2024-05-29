using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


using JJBA.Stands.Movement;
namespace JJBA.Stands.StarPlatinum.Controller
{
    public class SPController : MonoBehaviour
    {
        [SerializeField] private float _summonCooldown = 1f;

        [SerializeField] private GameObject _standModel;
        private Mover _mover;

        private bool _isActive;
        private float _summonTimer;

        public void SetActive(bool isActive)
        {
            if (_summonTimer <= _summonCooldown) return;

            _isActive = isActive;
            if (!_isActive) Hide();
            _summonTimer = 0f;
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public void Initialize()
        {
            _mover = GetComponent<Mover>();
            _summonTimer = _summonCooldown + 1f;
            SetActive(false);
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
            await _mover.Hide();
            _standModel.SetActive(false);
        }
    }
}
