using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace JJBA.Bootstrap
{
    public class WorldCanvasBootstrap : MonoBehaviour
    {
        private void Start() {
            GetComponent<Canvas>().worldCamera = Camera.main;
            GetComponent<LookAtConstraint>().AddSource(new ConstraintSource {
                 sourceTransform = Camera.main.transform,
                  weight = 1f });
        }
    }
}
