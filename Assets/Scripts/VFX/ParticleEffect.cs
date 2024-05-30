using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.VFX
{
    [System.Serializable]
    public class ParticleEffect
    {
        public string name;
        public GameObject particleEffect;
        public Transform parent;
        public bool attachToParent = false;

        [HideInInspector]
        public GameObject instance;
    }
}
