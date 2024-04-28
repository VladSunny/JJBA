using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace JJBA.UI
{
    public class DamagePopup : MonoBehaviour
    {
        public Vector3 randomizePositionFrom = new Vector3(1f, 1f, 0);
        public Vector3 randomizePositionTo = new Vector3(1f, 1f, 0);

        private TextMeshProUGUI damageText;

        private void OnEnable()
        {
            RandomizePosition();
        }

        public void Setup(float damageAmount)
        {
            damageText = GetComponentInChildren<TextMeshProUGUI>();
            damageText.text = "-" + damageAmount.ToString();
        }

        public void DestroyPopup()
        {
            Destroy(gameObject);
        }

        private void RandomizePosition()
        {
            transform.position = new Vector3
            (
                Random.Range(randomizePositionFrom.x, randomizePositionTo.x),
                Random.Range(randomizePositionFrom.y, randomizePositionTo.y),
                Random.Range(randomizePositionFrom.z, randomizePositionTo.z)
            );
        }
    }
}