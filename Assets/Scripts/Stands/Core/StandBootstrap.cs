using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JJBA.Stands.StarPlatinum.Bootstrap;
using JJBA.Stands.StarPlatinum.Input;
using JJBA.Stands.StarPlatinum.Controller;

namespace JJBA.Stands.Bootstrap
{
    public class StandBootstrap : MonoBehaviour
    {
        public GameObject standPrefab;
        private GameObject _stand;
        private Transform _playerOrientation;

        private void Awake()
        {
            _stand = Instantiate(standPrefab);
            _playerOrientation = transform.parent;

            _stand.GetComponent<SPBootstrap>().Initialize(_playerOrientation, transform);

            if (transform.GetComponent<PlayerInput>() != null)
            {
                gameObject.AddComponent<SPInput>();
                GetComponent<SPInput>().Initialize(_stand.GetComponent<SPController>());
            }
        }
    }
}
