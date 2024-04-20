using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using JJBA.Combat;

namespace JJBA.Core
{
    public class DynamicHitBox : MonoBehaviour
    {
        public Transform characterTransform;
        public LayerMask enemyLayer;

        [Header("Debug")]
        [SerializeField] private bool drawDebugBoxInEditor = true;
        [SerializeField] private bool debugPunch = true;
        [SerializeField] private KeyCode punchKey = KeyCode.Mouse0;
        [SerializeField] private bool debugDraw = true;


        public void CreateHitBox(Vector3 relativeBoxPosition, Vector3 boxSize, Action<Collider> actionOnHit,
            bool debugDraw = false)
        {
            Vector3 boxPosition = characterTransform.position + characterTransform.forward * relativeBoxPosition.z +
                                  characterTransform.right * relativeBoxPosition.x +
                                  characterTransform.up * relativeBoxPosition.y;

            Quaternion boxRotation = characterTransform.rotation;

            Collider[] hitColliders = Physics.OverlapBox(boxPosition, boxSize / 2, boxRotation, enemyLayer);

            foreach (Collider hitCollider in hitColliders)
            {
                actionOnHit?.Invoke(hitCollider);
            }

            if (debugDraw)
                DebugDrawBox(boxPosition, boxSize, boxRotation, Color.red, 2f);
        }


        private void DebugDrawBox(Vector3 position, Vector3 boxSize, Quaternion orientation, Color color,
            float duration)
        {
            Vector3 halfBoxSize = boxSize / 2.0f;

            Vector3[] points = new Vector3[8]
            {
                orientation * new Vector3(halfBoxSize.x, halfBoxSize.y, halfBoxSize.z),
                orientation * new Vector3(-halfBoxSize.x, halfBoxSize.y, halfBoxSize.z),
                orientation * new Vector3(-halfBoxSize.x, -halfBoxSize.y, halfBoxSize.z),
                orientation * new Vector3(halfBoxSize.x, -halfBoxSize.y, halfBoxSize.z),
                orientation * new Vector3(halfBoxSize.x, halfBoxSize.y, -halfBoxSize.z),
                orientation * new Vector3(-halfBoxSize.x, halfBoxSize.y, -halfBoxSize.z),
                orientation * new Vector3(-halfBoxSize.x, -halfBoxSize.y, -halfBoxSize.z),
                orientation * new Vector3(halfBoxSize.x, -halfBoxSize.y, -halfBoxSize.z)
            };

            for (int i = 0; i < 4; i++)
            {
                Debug.DrawLine(position + points[i], position + points[(i + 1) % 4], color, duration);
                Debug.DrawLine(position + points[i + 4], position + points[((i + 1) % 4) + 4], color, duration);
                Debug.DrawLine(position + points[i], position + points[i + 4], color, duration);
            }
        }


        private void Update()
        {
            if (debugPunch && Input.GetKeyDown(punchKey))
            {
                CreateHitBox(Vector3.forward * 1f, new Vector3(1f, 1f, 1f), (collider) =>
                {
                    Debug.Log(collider);
                }, debugDraw);
            }
        }

        private void OnDrawGizmos()
        {
            if (characterTransform != null && drawDebugBoxInEditor)
            {
                Vector3 relBoxPos = Vector3.forward * 1f;
                Vector3 boxSize = new Vector3(1f, 1f, 1f);

                Vector3 boxPosition = characterTransform.position + characterTransform.forward * relBoxPos.z +
                                      characterTransform.right * relBoxPos.x +
                                      characterTransform.up * relBoxPos.y;
                Quaternion boxRotation = characterTransform.rotation;

                DebugDrawBox(boxPosition, boxSize, boxRotation, Color.blue, 0);
            }
        }
    }
}