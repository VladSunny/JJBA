using UnityEngine;

namespace JJBA.Combat
{
    public enum DamageType
    {
        NONE,
        SLAP,
        BASE,
        PUNCH_FINISHER,
    }

    public class Damage
    {
        public float damageValue;
        public Vector3 from = Vector3.zero;
        public Vector3 forse = Vector3.zero;
        public DamageType type = DamageType.NONE;
    }
}
