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

        private void OnTriggerEnter(Collider other) {
            if (((1 << other.gameObject.layer) & creature) == 0) return;
            GameObject character = other.gameObject;
            
            RagdollSystem characterRagdollSystem = character.GetComponent<RagdollSystem>();
            characterRagdollSystem.Fall();
            
            Rigidbody hipsRb = characterRagdollSystem.hipsBone.GetComponent<Rigidbody>();
            hipsRb.AddForce(transform.up * force, ForceMode.Impulse);
            

            // Debug.Log("Trampoline collided with: " + other.gameObject.name);
            // other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}
