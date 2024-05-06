using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;
using JJBA.Movement;
using JJBA.Ragdoll;
using JJBA.Control;

namespace JJBA.Bootstrap
{
    [RequireComponent(typeof(RagdollSystem))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(StandlessFighter))]
    [RequireComponent(typeof(HitHandler))]

    public class EnemyBootstrap : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<RagdollSystem>().Initialize();
            GetComponent<Health>().Initialize();
            GetComponent<HitHandler>().Initialize();
            GetComponent<Mover>().Initialize();
            GetComponent<StandlessFighter>().Initialize();
        }
    }   
}
