using EnumCollection;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.Entity;

namespace Manager
{
    public class PlayerContainer
    {
        private bool isAlive;

        #region Properties
        public float Gold                               { get; private set; }
        public List<AIAgent> Agents                     { get; private set; }
        public List<Construct> Constructs               { get; private set; }
        public List<ConstructId> ConstructsCantBuild    { get; private set; }
        
        public bool IsCanBuild(ConstructId type)
        {
            return ConstructsCantBuild.Contains(type);
        }

        public bool IsAlive
        {
            get { return Agents.Count + Constructs.Count > 0; }
            private set { isAlive = value; }                
        }
        #endregion

        public PlayerContainer()
        {
            Gold = 2500;            
            Agents = new List<AIAgent>();
            Constructs = new List<Construct>();
            ConstructsCantBuild = new List<ConstructId>();
            isAlive = true;
        }                 

        private void NewConstructBuilt(Construct construct)
        {
            ConstructId[] unlock = construct.Owned;
            for (int i = 0; i < unlock.Length; i++)
            {
                ConstructsCantBuild.Add(unlock[i]);
            }
        }

        private void ConstructDestroyed(Construct construct)
        {
            ConstructId[] unlock = construct.Owned;
            for (int i = 0; i < unlock.Length; i++)
            {
                ConstructsCantBuild.RemoveAt(ConstructsCantBuild.IndexOf((unlock[i])));

            }
        }
        public void TakeGold(float gold)
        {
            Gold += gold;
        }

        public PayGoldStatus PayGold(float requireGold, out float debt)
        {
            debt = requireGold;
            if (Gold <= 0) return PayGoldStatus.Terminal;
            if (Gold < requireGold)
            {
                debt = requireGold - Gold;
                return PayGoldStatus.Pause;
            }
            debt = 0;
            return PayGoldStatus.Success;
        }

        // agent
        public void AddAgent(AIAgent agent)
        {
            Agents.Add(agent);
        }
        public void RemoveAgent(AIAgent agent)
        {
            int index = Agents.IndexOf(agent);
            if (index >= 0)
            {
                Agents.RemoveAt(index);
            }
        }
        // AI uitls
        public AIAgent[] GetNeighbours(AIAgent agent)
        {
            List<AIAgent> result = new List<AIAgent>();
            float sqrBoundRadius = agent.NeighbourRadius * agent.NeighbourRadius;
            for (int i = 0; i < Agents.Count; i++)
            {
                if (Agents[i] != agent &&
                    Vector3.SqrMagnitude(agent.Position - Agents[i].Position) <= sqrBoundRadius)
                {
                    result.Add(Agents[i]);
                }
            }
            return result.ToArray();
        }
        // constructs
        public void AddConstruct(Construct construct)
        {
            Constructs.Add(construct);
            NewConstructBuilt(construct);
        }
        public void RemoveConstruct(Construct construct)
        {
            int index = Constructs.IndexOf(construct);
            if (index >= 0)
            {
                Constructs.RemoveAt(index);
                ConstructDestroyed(construct);
            }

        }

        public Construct GetConstruct(System.Type type)
        {
            int count = Constructs.Count;
            for (int i = 0; i < count; i++)
            {
                if(Constructs[i].GetType() == type)
                {
                    return Constructs[i];
                }
            }
            return null;
        }
    }
}
