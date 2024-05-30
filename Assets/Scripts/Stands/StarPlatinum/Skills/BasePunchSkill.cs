using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
namespace JJBA.Stands.StarPlatinum.Skill
{
    public class BasePunchSkill : MonoBehaviour
    {
        private Mover _mover;
        private Transform _skillPosition;

        public void Initialize(Transform skillPosition)
        {
            _mover = GetComponent<Mover>();
            _skillPosition = skillPosition;
        }

        public void Use()
        {
            _mover.SetTarget(_skillPosition);
        }
    }
}
