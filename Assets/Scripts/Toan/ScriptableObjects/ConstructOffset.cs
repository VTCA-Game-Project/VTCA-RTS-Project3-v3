using UnityEngine;

namespace RTS_ScriptableObject
{
    [CreateAssetMenu(fileName = "Construct Offset", menuName = @"RTS Game/Defender Offset", order = 2)]
    public class ConstructOffset : ScriptableObject
    {
        public int MaxHP;

        // defender
        public int Damage;
        public float AttackRadius;
        public float DelayAttack;
        public float AttackForce;

    }
}