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
        [SerializeField] private GameObject _standModel;
        private Mover _mover;
        
        private bool _isActive;

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            if (!_isActive) Hide();
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public void Initialize() {
            _mover = GetComponent<Mover>();
            
            SetActive(false);
        }

        private void Update()
        {
            if (_isActive)
            {
                _standModel.SetActive(true);
            }
        }

        private async void Hide()
        {
            await _mover.Hide();
            _standModel.SetActive(false);
        }
    }
}
