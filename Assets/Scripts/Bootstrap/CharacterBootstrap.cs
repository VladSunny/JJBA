using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;
using JJBA.Movement;
using JJBA.Ragdoll;
using JJBA.UI;

namespace JJBA.Bootstrap
{
    [RequireComponent(typeof(RagdollSystem))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(StandlessFighter))]
    [RequireComponent(typeof(HitHandler))]
    [RequireComponent(typeof(FallDamage))]
    [RequireComponent(typeof(ShowDamagePopup))]

    public class CharacterBootstrap : MonoBehaviour
    {
        protected virtual void Start()
        {
            GetComponent<RagdollSystem>().Initialize();
            GetComponent<Health>().Initialize();
            GetComponent<FallDamage>().Initialize();
            GetComponent<ShowDamagePopup>().Initialize();
            GetComponent<HitHandler>().Initialize();
            GetComponent<Mover>().Initialize();
            GetComponent<StandlessFighter>().Initialize();
        }
    }
}
