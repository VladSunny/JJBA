using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Core
{
    public class ToggleVisibility : MonoBehaviour
    {
        private Renderer[] _renderers;

        private void Start()
        {
            _renderers = GetComponentsInChildren<Renderer>();
        }

        public void SetVisibility(bool isVisible)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.enabled = isVisible;
            }
        }
    }
}
