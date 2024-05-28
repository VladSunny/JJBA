using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Bootstrap
{
    public class StandBootstrap : MonoBehaviour
    {
        public GameObject standPrefab;
        private GameObject _stand;
        private Transform _playerOrientation;

        private void Awake()
        {
            _stand = Instantiate(standPrefab);
            _stand.GetComponent<DOTweenTest>().target = transform;

            _playerOrientation = transform.parent;
        }

        private void Update() {
            _stand.transform.forward = _playerOrientation.forward;
        }
    }
}
