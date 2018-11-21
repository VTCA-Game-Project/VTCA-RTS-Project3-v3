using UnityEngine;

namespace RTS_ScriptableObject
{
    [CreateAssetMenu(fileName = "Location Offset", menuName = @"RTS Game/Location Offset", order = 2)]
    public class ConstructLocationOffset : ScriptableObject
    {
        public Vector3 Yard;
        public Vector3 Refinery;
        public Vector3 Barrack;
        public Vector3[] Defender;
    }
}