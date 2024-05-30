using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
namespace JJBA.Stands.StarPlatinum.Skill
{
    public class BasePunchSkill : MonoBehaviour
    {
        private StandMover _mover;

        public void Initialize()
        {
            _mover = GetComponent<StandMover>();
        }

        public void Use()
        {
            _mover.UsingSkill();
        }
    }
}
