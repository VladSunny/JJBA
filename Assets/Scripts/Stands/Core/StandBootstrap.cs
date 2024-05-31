using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JJBA.Stands.StarPlatinum.Bootstrap;
using JJBA.Stands.StarPlatinum.Input;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.Definition;

namespace JJBA.Stands.Bootstrap
{
    public class StandBootstrap : MonoBehaviour
    {
        public GameObject standPrefab;
        private GameObject _stand;
        private Transform _playerOrientation;
        private GameObject _user;
        [SerializeField] private Transform _skillPosition;

        private void Awake()
        {
            if (GetComponentInParent<StandUser>() == null)
            {
                Debug.LogError("User not found");
                return;
            }

            _user = GetComponentInParent<StandUser>().gameObject;

            _playerOrientation = transform.parent;

            _stand = Instantiate(standPrefab, _playerOrientation.position, _playerOrientation.rotation);
            _stand.GetComponent<SPBootstrap>().Initialize(_playerOrientation, transform, _skillPosition, _user);

            if (transform.GetComponent<PlayerInput>() != null)
            {
                gameObject.AddComponent<SPInput>();
                GetComponent<SPInput>().Initialize(_stand.GetComponent<SPController>(), _stand);
            }
        }

        public bool isActive()
        {
            return _stand.GetComponent<SPController>().IsActive();
        }
    }
}
