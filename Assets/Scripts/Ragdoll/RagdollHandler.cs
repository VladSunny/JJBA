using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;

namespace JJBA.Ragdoll
{
    public class RagdollHandler : MonoBehaviour
    {
        private List<Rigidbody> rigidbodies;
        private List<Collider> colliders;

        void Awake()
        {
            rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
            colliders = new List<Collider>(GetComponentsInChildren<Collider>());
        }

        public void Enable()
        {
            foreach (Rigidbody rb in rigidbodies)
                rb.isKinematic = false;

            foreach (Collider col in colliders)
                col.enabled = true;
        }

        public void Disable()
        {
            foreach (Rigidbody rb in rigidbodies)
                rb.isKinematic = true;

            foreach (Collider col in colliders)
                if (!col.isTrigger) col.enabled = false;
        }
    }
}