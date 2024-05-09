using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Ragdoll.Test
{
    [RequireComponent(typeof(BoxCollider))]

    public class Trampoline : MonoBehaviour
    {
        [SerializeField] private LayerMask creature;
        [SerializeField] private float force = 100f;

        private void OnCollisionEnter(Collision collision) {
            if (((1 << collision.gameObject.layer) & creature) == 0) return;
            GameObject character = collision.gameObject;
            
            RagdollSystem characterRagdollSystem = character.GetComponentInParent<RagdollSystem>();
            characterRagdollSystem.Fall();
            
            Rigidbody hipsRb = characterRagdollSystem.hipsBone.GetComponent<Rigidbody>();
            hipsRb.AddForce(transform.up * force, ForceMode.Impulse);
        }
    }
}
