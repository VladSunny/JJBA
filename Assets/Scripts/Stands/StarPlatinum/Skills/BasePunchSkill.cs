using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
using Unity.VisualScripting;
using System.Threading.Tasks;
namespace JJBA.Stands.StarPlatinum.Skill
{
    public class BasePunchSkill : MonoBehaviour
    {
        private float _cooldown = 0.5f;
        private StandMover _mover;

        public void Initialize()
        {
            _mover = GetComponent<StandMover>();
        }

        public async void Use()
        {
            _mover.UsingSkill();

            await Task.Delay((int)(_cooldown * 1000));

            _mover.Idle();
        }
    }
}
