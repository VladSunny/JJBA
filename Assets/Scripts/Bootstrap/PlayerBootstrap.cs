using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Combat;
using JJBA.Movement;
using JJBA.Ragdoll;
using JJBA.Control;

namespace JJBA.Bootstrap
{
    [RequireComponent(typeof(PlayerController))]

    public class PlayerBootstrap : CharacterBootstrap
    {
        protected override void Start()
        {
            base.Start();
            GetComponent<PlayerController>().Initialize();
        }
    }   
}
