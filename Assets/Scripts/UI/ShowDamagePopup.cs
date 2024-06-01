using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;
using UnityEngine.Animations;
using System;

namespace JJBA.UI
{
    public class ShowDamagePopup : MonoBehaviour
    {
        [SerializeField] private GameObject damagePopupPrefab;
        [SerializeField] private Transform popupParent;
        [SerializeField] private bool drawGizmos;

        private Health _health;

        public void Initialize()
        {
            _health = GetComponentInParent<Health>();
            _health.onHealthDamaged.AddListener(createPopup);
        }

        public void createPopup(Damage damage)
        {
            if (damagePopupPrefab == null)
            {
                Debug.LogError("No text popup prefab set on ShowDamagePopup");
                return;
            }

            float roundedDamage = Mathf.Round(damage.damageValue * 10f) / 10f;

            ConstraintSource source = new ConstraintSource { sourceTransform = Camera.main.transform, weight = 1f };
            GameObject popup = Instantiate(damagePopupPrefab, Vector3.zero, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Initialize(roundedDamage);
            popup.GetComponent<LookAtConstraint>().AddSource(source);
            popup.transform.SetParent(popupParent, false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            if (damagePopupPrefab == null)
            {
                Debug.LogWarning("DamagePopupPrefab is null, can't draw gizmo for ShowDamagePopup");
                return;
            }

            DamagePopup damagePopup = damagePopupPrefab.GetComponent<DamagePopup>();

            if (damagePopup == null)
            {
                Debug.LogWarning("DamagePopup component is null, can't draw gizmo for ShowDamagePopup");
                return;
            }

            Gizmos.color = new Color(1, 0, 0, 0.5f);

            Vector3 center = (damagePopup.randomizePositionFrom + damagePopup.randomizePositionTo) * 0.5f;
            Vector3 size = new Vector3
            (
                Mathf.Abs(damagePopup.randomizePositionFrom.x - damagePopup.randomizePositionTo.x),
                Mathf.Abs(damagePopup.randomizePositionFrom.y - damagePopup.randomizePositionTo.y),
                Mathf.Abs(damagePopup.randomizePositionFrom.z - damagePopup.randomizePositionTo.z)
            );

            Gizmos.DrawCube(center, size);
        }
#endif
    }
}
