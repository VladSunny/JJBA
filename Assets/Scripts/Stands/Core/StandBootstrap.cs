using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.StarPlatinum.Bootstrap;
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

            _stand.GetComponent<StarPlatinumBootstrap>().Initialize(_playerOrientation, transform);
        }
    }
}
