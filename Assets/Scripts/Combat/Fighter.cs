using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using JJBA.Core;

namespace JJBA.Combat
{
    [RequireComponent(typeof(DynamicHitBox))]
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private bool drawHitBox;
        
        private DynamicHitBox _dynamicHitBox;
        private Animator _animator;
        private static readonly int Punch = Animator.StringToHash("basePunch");

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void BasePunch()
        {
            _animator.SetTrigger(Punch);
            _dynamicHitBox.CreateHitBox(Vector3.forward * 1f, new Vector3(1f, 1f, 1f), (collider) =>
            {
                Debug.Log(collider);
            }, drawHitBox);
        }
    }
}
