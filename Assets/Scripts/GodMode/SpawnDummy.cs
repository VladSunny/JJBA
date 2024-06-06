using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.GodMode
{
    public class SpawnDummy : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] float _offset;
        [SerializeField] KeyCode _spawnKey = KeyCode.Alpha3;

        private void Update()
        {
            if (Input.GetKeyDown(_spawnKey))
                Spawn();
        }

        private void Spawn()
        {
            Instantiate(_prefab, transform.position + Vector3.up * _offset, Quaternion.identity);
        }
    }
}
