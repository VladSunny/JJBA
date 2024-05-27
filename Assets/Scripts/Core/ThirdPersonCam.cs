using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Ragdoll;
using Cinemachine;
namespace JJBA.Core
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform playerObj;
        [SerializeField] private Transform combatLookAt;
        [SerializeField] private RagdollSystem ragdollSystem;
        [SerializeField] private Transform hipsBoneTransform;
        [SerializeField] private Transform playerTransform;

        [Header("Cameras")]
        [SerializeField] private CinemachineFreeLook CombatCamera;
        // [SerializeField] private CinemachineFreeLook DeathCamera;


        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            CombatCamera.Priority = 10;
            // DeathCamera.Priority = 0;

            CombatCamera.Follow = playerTransform;
            CombatCamera.LookAt = combatLookAt;
        }

        private void Update()
        {
            if (ragdollSystem != null && ragdollSystem.IsFall())
            {
                // CombatCamera.Priority = 0;
                CombatCamera.Follow = hipsBoneTransform;
                CombatCamera.LookAt = hipsBoneTransform;
                // DeathCamera.Priority = 10;
            }
            else
            {
                // CombatCamera.Priority = 10;
                // DeathCamera.Priority = 0;

                CombatCamera.Follow = playerTransform;
                CombatCamera.LookAt = combatLookAt;

                Vector3 dirToCombatLookAt = combatLookAt.position -
                                        new Vector3(transform.position.x, combatLookAt.position.y,
                                            transform.position.z);
                orientation.forward = dirToCombatLookAt.normalized;
                playerObj.forward = dirToCombatLookAt.normalized;
            }
        }

    }
}