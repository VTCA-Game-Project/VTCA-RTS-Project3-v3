using UnityEngine;

namespace RTS_ScriptableObject
{
    [CreateAssetMenu(fileName = "Construct Price", menuName = @"RTS Game/Construct Price", order = 4)]
    public class ConstructPrice : ScriptableObject
    {
        public int Yard;
        public int Barrack;
        public int Refinery;
        public int Defender;    
    }
}