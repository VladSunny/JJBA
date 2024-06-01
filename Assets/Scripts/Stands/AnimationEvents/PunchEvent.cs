using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JJBA
{
    public class PunchEvent : MonoBehaviour
    {
        public UnityEvent punchEvent;

        public void Punch()
        {
            punchEvent.Invoke();
        }
    }
}
