using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Core
{
    public class CopyPos : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            transform.position = target.position;
        }
    }
}
