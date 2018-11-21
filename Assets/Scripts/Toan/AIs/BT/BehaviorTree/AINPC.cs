using Manager;
using RTS_ScriptableObject;
using UnityEngine;

namespace AIs.BT.BehaviorTree
{
    public class AINPC : MonoBehaviour {

        private BTNPCPlayer decisionTree;

        public ConstructLocationOffset LocationOffset;
        public ConstructPrice ConstructPrice;
        public GameEntityPrice AgentPrice;

        private void Start()
        {
            MainPlayer player = FindObjectOfType<MainPlayer>();
            NPCPlayer npc = GetComponent<NPCPlayer>();
            decisionTree = new BTNPCPlayer(player: npc, main: player)
            {
                LocationOffset  = this.LocationOffset,
                ConstructPrice  = this.ConstructPrice,
                AgentPrice      = this.AgentPrice,
            };           
        }

        private void Update()
        {
            decisionTree.UpdateCountDown(Time.deltaTime);
            decisionTree.Evaluate();
        }
    }
}