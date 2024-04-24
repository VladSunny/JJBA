using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JJBA.Combat.Events {
    public class StandlessEvents : MonoBehaviour
    {
        public UnityEvent onBasePunch;

        void BasePunch() {
            onBasePunch.Invoke();
        }
    }
}
