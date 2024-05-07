using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Core
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask ground;
        private bool grounded = false;

        public bool IsGrounded() { return grounded; }

        private void OnTriggerEnter(Collider other) {
            if (((1 << other.gameObject.layer) & ground) != 0)
            {
                grounded = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (((1 << other.gameObject.layer) & ground) != 0)
            {
                grounded = false;
            }
        }
    }   
}
