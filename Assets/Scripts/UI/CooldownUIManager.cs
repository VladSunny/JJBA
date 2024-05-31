using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.UI
{
    public class CooldownUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _cooldownTimerPrefab;
        [SerializeField] private Transform _cooldownTimerParent;

        public void AddCooldownTimer(float cooldown, string text)
        {
            GameObject timerGO = Instantiate(_cooldownTimerPrefab, _cooldownTimerParent);
            timerGO.GetComponent<CooldownTimer>().Initialize(cooldown, text);
        }
    }
}
