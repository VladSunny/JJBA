using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Ragdoll;
namespace JJBA.Core
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")] public Transform orientation;
        [SerializeField] private Transform playerObj;
        [SerializeField] private Transform combatLookAt;
        [SerializeField] private RagdollSystem ragdollSystem;


        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (ragdollSystem != null && ragdollSystem.IsFall()) return;

            Vector3 dirToCombatLookAt = combatLookAt.position -
                                        new Vector3(transform.position.x, combatLookAt.position.y,
                                            transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }

    }
}