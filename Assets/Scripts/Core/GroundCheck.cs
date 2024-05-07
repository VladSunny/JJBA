using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Core
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask ground;
        private int groundCollisions = 0;

        public bool IsGrounded() { return groundCollisions > 0; }


        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & ground) != 0)
            {
                groundCollisions++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & ground) != 0)
            {
                groundCollisions--;
            }
        }

    }   
}
