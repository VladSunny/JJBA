using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.Core
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")] public Transform orientation;
        public Transform playerObj;
        public Transform combatLookAt;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Vector3 dirToCombatLookAt = combatLookAt.position -
                                        new Vector3(transform.position.x, combatLookAt.position.y,
                                            transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;
            
            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }
}