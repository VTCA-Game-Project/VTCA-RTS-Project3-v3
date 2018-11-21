using UnityEngine;

namespace RTS_ScriptableObject
{
    [CreateAssetMenu(fileName = "Agent Price", menuName = @"RTS Game/Agent Price", order = 3)]
    public class GameEntityPrice : ScriptableObject
    {
        public int Warrior;
        public int Magic;
        public int OrcTanker;
        public int Archer;
        public int WoodHorse;
        public int HumanWarrior;        
    }
}