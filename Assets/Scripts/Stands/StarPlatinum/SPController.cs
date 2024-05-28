using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Stands.StarPlatinum.Controller
{
    public class SPController : MonoBehaviour
    {
        [SerializeField] private GameObject _standModel;
        private bool _isActive = false;

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public bool GetActive()
        {
            return _isActive;
        }

        private void Update()
        {
            if (_isActive)
            {
                _standModel.SetActive(true);
            }
            else
            {
                _standModel.SetActive(false);
            }
        }
    }
}
