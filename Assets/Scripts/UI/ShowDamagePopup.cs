using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;
using UnityEngine.Animations;

namespace JJBA.UI {
    public class ShowDamagePopup : MonoBehaviour
    {
        [SerializeField] private GameObject textPopupPrefab;

        private Health _health;

        private void Start()
        {
            _health = GetComponent<Health>();
            _health.onHealthDamaged.AddListener(createPopup);
        }

        public void createPopup(float damage) {
            Debug.Log("Popup: " + damage);

            if (textPopupPrefab == null) {
                Debug.LogError("No text popup prefab set on ShowDamagePopup");
                return;
            }

            ConstraintSource source = new ConstraintSource { sourceTransform = Camera.main.transform, weight = 1f };

            GameObject popup = Instantiate(textPopupPrefab, transform.position, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage);
            popup.GetComponent<LookAtConstraint>().AddSource(source);
            popup.transform.SetParent(transform.parent, false);
        }
    }
}
