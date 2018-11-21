using AIs.BT.Commoms;
using Common;
using Common.Building;
using Common.Entity;
using EnumCollection;
using Manager;
using Pattern;
using RTS_ScriptableObject;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace AIs.BT.BehaviorTree
{
    public class BTNPCPlayer
    {
        private readonly int NumAgentToAttack = 10;
        private readonly Vector3 WayPoint = new Vector3(48, 0, 48);      
        private readonly Dictionary<ConstructId, float> ConstructBuyDelay = new Dictionary<ConstructId, float>()
        {
            { ConstructId.Yard      , 5.0f },
            { ConstructId.Refinery  , 5.0f },
            { ConstructId.Barrack   , 5.0f },
            { ConstructId.Defender  , 5.0f },
            { ConstructId.Radar     , 5.0f },
        };
        private readonly Dictionary<Soldier, float> AgentBuyDelay = new Dictionary<Soldier, float>()
        {
            { Soldier.Archer        ,3.0f},
            { Soldier.HumanWarrior  ,3.0f},
            { Soldier.WoodHorse     ,3.0f},
            { Soldier.Warrior       ,3.0f},
            { Soldier.OrcTanker     ,3.0f},
            { Soldier.Magic         ,3.0f},
        };

        private readonly Soldier[] OrcAgent = new Soldier[]
        {
            Soldier.Warrior,
            Soldier.Magic,
            Soldier.OrcTanker,
        };
        private readonly Soldier[] HumanAgent = new Soldier[]
       {
            Soldier.Archer,
            Soldier.WoodHorse,
            Soldier.HumanWarrior
       };

        private float createAgentDelayCounter;
        private bool isAttack;
        private bool isUpdatedWarrior;
        private bool isUpdatedRange;
        private bool isUpdatedTank;

        private GameEntity currentTarget;
        private Soldier typeAgentWantToBuy;
        private NPCPlayer npc;
        private MainPlayer mainPlayer;

        private NodeState attackWaveState = NodeState.Failure;
        private QueryList<ConstructId, float> constructCountQuery;
        private QueryList<Soldier, float> agentCountQuery;

        public ConstructLocationOffset LocationOffset { get; set; }
        public ConstructPrice ConstructPrice { get; set; }
        public GameEntityPrice AgentPrice { get; set; }

        public BaseNode Root { get; private set; }

        public BTNPCPlayer(NPCPlayer player, MainPlayer main)
        {
            npc = player;
            mainPlayer = main;

            constructCountQuery = new QueryList<ConstructId, float>();
            agentCountQuery = new QueryList<Soldier, float>();

            InitBT();
            ResetAgentCheckUpdated();
        }

        public void UpdateCountDown(float deltaTime)
        {
            createAgentDelayCounter += deltaTime;
            List<QueryItem<ConstructId, float>> queryItems = constructCountQuery.QueryItemList();
            for (int i = 0; i < queryItems.Count; i++)
            {
                queryItems[i].value += deltaTime;
            }

            List<QueryItem<Soldier, float>> agentItems = agentCountQuery.QueryItemList();
            for (int i = 0; i < agentItems.Count; i++)
            {
                if ((agentItems[i].key == Soldier.Warrior || agentItems[i].key == Soldier.HumanWarrior)
                    && !isUpdatedWarrior)
                {
                    agentItems[i].value += deltaTime;
                    isUpdatedWarrior = true;
                    continue;
                }
                if ((agentItems[i].key == Soldier.Archer || agentItems[i].key == Soldier.Magic)
                    && !isUpdatedRange)
                {
                    agentItems[i].value += deltaTime;
                    isUpdatedRange = true;
                    continue;
                }
                if ((agentItems[i].key == Soldier.OrcTanker || agentItems[i].key == Soldier.WoodHorse)
                    && !isUpdatedTank)
                {
                    agentItems[i].value += deltaTime;
                    isUpdatedTank = true;
                    continue;
                }

                if (isUpdatedTank && isUpdatedWarrior & isUpdatedRange)
                {
                    break;
                }
            }
            ResetAgentCheckUpdated();
            InstantiateAgent();
        }

        public NodeState Evaluate()
        {
            return Root.Evaluate();
        }

        private void InitBT()
        {
            Root = new Selector(new List<BaseNode>()
            {
                new ActionNode(CheckGameTerminalAction),
                ClearAllUnitProgressSequence(),
            });
        }

        private void ResetAgentCheckUpdated()
        {
            isUpdatedRange = false;
            isUpdatedTank = false;
            isUpdatedWarrior = false;
        }

        private void InstantiateAgent()
        {
            if(npc.GetConstruct(typeof(Barrack)) == null)
            {
                agentCountQuery.Clear();
                return;
            }
            List<QueryItem<Soldier, float>> agentItems = agentCountQuery.QueryItemList();
            for (int i = 0; i < agentItems.Count; i++)
            {
                if (agentItems[i].value >= AgentBuyDelay[agentItems[i].key])
                {
                    if (CheckEnoughGoldToBuyAgent(agentItems[i].key) == NodeState.Success && createAgentDelayCounter > 0.2f)
                    {
                        CreateAgent(agentItems[i].key);
                        agentCountQuery.RemoveAt(i);
                        createAgentDelayCounter = 0.0f;
                        break;
                    }
                }
            }
        }

        private void PayAgentGold(Soldier type)
        {
            int price = 0;
            switch (type)
            {
                case Soldier.Archer:
                    price = AgentPrice.Archer;
                    break;
                case Soldier.HumanWarrior:
                    price = AgentPrice.HumanWarrior;
                    break;
                case Soldier.Magic:
                    price = AgentPrice.Magic;
                    break;
                case Soldier.OrcTanker:
                    price = AgentPrice.OrcTanker;
                    break;
                case Soldier.Warrior:
                    price = AgentPrice.Warrior;
                    break;
                case Soldier.WoodHorse:
                    price = AgentPrice.WoodHorse;
                    break;

            }
            npc.TakeGold(-price);
        }

        private void CreateAgent(Soldier type)
        {
            GameObject prefab = AssetUtils.Instance.GetAsset(type.ToString()) as GameObject;
            Construct barrack = npc.GetConstruct(typeof(Barrack));
            if (prefab != null && barrack != null)
            {
                AIAgent agent = GameObject.Instantiate(prefab, barrack.transform.root.position, Quaternion.identity).GetComponent<AIAgent>();
                agent.Owner = npc;
                agent.gameObject.SetActive(true);
                agent.SetTarget(TargetType.Place, Vector3.ProjectOnPlane(barrack.transform.position + barrack.transform.forward * 8, Vector3.up),null);
            }
        }
        //  commons action node
        private NodeState CheckGameTerminalAction()
        {
            if (!npc.IsAlive() || !mainPlayer.IsAlive()) return NodeState.Success;
            return NodeState.Failure;
        }

        private NodeState CheckGameStatus()
        {
            if (UpdateGameStatus.Instance.GameIsRunning)
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private NodeState CheckEnoughGold(int gold)
        {
            if (npc.GetGold() >= gold) return NodeState.Success;
            return NodeState.Failure;
        }

        private NodeState SetConstructPosition(ConstructId type, Vector3 pos, Transform construct)
        {
            if (constructCountQuery.Remove(type))
            {
                construct.position = pos;
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private NodeState CheckConstructCountdown(ConstructId type)
        {
            float time;
            if (constructCountQuery.TryGetValue(type, out time))
            {
                if (time >= ConstructBuyDelay[type]) return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private NodeState CheckAgentCountdown(Soldier type)
        {
            float time;
            if (agentCountQuery.TryGetValue(type, out time))
            {
                if (time >= AgentBuyDelay[type]) return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private NodeState CheckUnclockedConstruct(ConstructId type)
        {
            if (npc.IsCanBuild(type))
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private NodeState CheckContainConstruct(System.Type constructType)
        {
            List<Construct> constructs = npc.Constructs;
            for (int i = 0; i < constructs.Count; i++)
            {
                if (constructs[i].GetType() == constructType)
                {
                    return NodeState.Success;
                }
            }
            return NodeState.Failure;
        }

        private NodeState CheckAllBuyProgressSuccess(System.Type type)
        {
            if (type == typeof(ConstructId))
            {
                List<QueryItem<ConstructId, float>> queryItems = constructCountQuery.QueryItemList();
                for (int i = 0; i < queryItems.Count; i++)
                {
                    if (queryItems[i].value < ConstructBuyDelay[queryItems[i].key]) return NodeState.Failure;
                }
                return NodeState.Success;
            }

            if (type == typeof(Soldier))
            {
                List<QueryItem<Soldier, float>> agentItems = agentCountQuery.QueryItemList();
                //for (int i = 0; i < agentItems.Count; i++)
                //{
                //    if (agentItems[i].value < AgentBuyDelay[agentItems[i].key]) return NodeState.Failure;
                //}
                if (agentItems.Count <= 0)
                    return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private NodeState MoveAgentsTo(Vector3 pos, TargetType targetType, GameEntity gameEntity)
        {
            List<AIAgent> agents = npc.Agents;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].SetTarget(targetType, pos, gameEntity);
            }
            return NodeState.Success;
        }

        private NodeState CheckAgentsReachedTarget()
        {
            List<AIAgent> agent = npc.Agents;
            for (int i = 0; i < agent.Count; i++)
            {
                if (!agent[i].IsReachedTarget) return NodeState.Failure;
            }
            return NodeState.Success;
        }

        private NodeState CountAgentsToAttack()
        {
            if (npc.Agents.Count >= NumAgentToAttack) return NodeState.Success;
            return NodeState.Failure;
        }

        private NodeState BuyConstruct(ConstructId type)
        {
            if (!constructCountQuery.ContainsKey(type))
            {
                constructCountQuery.Add(new QueryItem<ConstructId, float>(type, 0.0f), true);
                int price = 0;
                switch (type)
                {
                    case ConstructId.Barrack:
                        price = ConstructPrice.Barrack;
                        break;
                    case ConstructId.Defender:
                        price = ConstructPrice.Defender;
                        break;
                    case ConstructId.Refinery:
                        price = ConstructPrice.Refinery;
                        break;
                    case ConstructId.Yard:
                        price = ConstructPrice.Yard;
                        break;
                }
                npc.TakeGold(-price);
            }
            return NodeState.Success;
        }

        private NodeState BuyAgent(Soldier type)
        {
            if (!agentCountQuery.ContainsKey(type))
            {
                if (CheckEnoughGoldToBuyAgent(type) == NodeState.Success)
                {
                    agentCountQuery.Add(new QueryItem<Soldier, float>(type, 0.0f), false);
                    return NodeState.Success;
                }
                else
                {
                    return NodeState.Failure;
                }
            }
            return NodeState.Success;
        }

        private NodeState CheckEnoughGoldToBuyAgent(Soldier type)
        {
            switch (type)
            {
                case Soldier.Archer:
                    return CheckEnoughGold(AgentPrice.Archer);
                case Soldier.HumanWarrior:
                    return CheckEnoughGold(AgentPrice.HumanWarrior);
                case Soldier.Magic:
                    return CheckEnoughGold(AgentPrice.Magic);
                case Soldier.OrcTanker:
                    return CheckEnoughGold(AgentPrice.OrcTanker);
                case Soldier.Warrior:
                    return CheckEnoughGold(AgentPrice.Warrior);
                case Soldier.WoodHorse:
                    return CheckEnoughGold(AgentPrice.WoodHorse);
                default:
                    return NodeState.Failure;
            }
        }
        // tree node

        #region Location Yard Construct Sequence
        private Sequence LocationYardConstructNode()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(CheckYardEndCountDown),
                new ActionNode(SetYardContrustPostion),
            });
        }

        private NodeState SetYardContrustPostion()
        {
            GameObject prefab;
            if (Singleton.classname == "Human")
            {
                prefab = AssetUtils.Instance.GetAsset("OrcYard") as GameObject;
            }
            else
            {
                prefab = AssetUtils.Instance.GetAsset("HumanYard") as GameObject;
            }

            GameObject yardGameObj = GameObject.Instantiate(prefab);
            Construct yard = yardGameObj.GetComponentInChildren<Construct>();
            yard.Player = npc;
            yard.Group = Group.NPC;
            yardGameObj.SetActive(true);
            yard.Build();

            return SetConstructPosition(ConstructId.Yard, LocationOffset.Yard, yardGameObj.transform);
        }

        private NodeState CheckYardEndCountDown()
        {
            return CheckConstructCountdown(ConstructId.Yard);
        }
        #endregion

        #region Location Refinery Construct Sequence
        private Sequence LocationRefineryConstructNode()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(CheckRefineryEndCountDown),
                new ActionNode(SetRefineryConstructPosition),
            });
        }

        private NodeState SetRefineryConstructPosition()
        {
            GameObject prefab;
            if (Singleton.classname == "Human")
            {
                prefab = AssetUtils.Instance.GetAsset("OrcRefinery") as GameObject;
            }
            else
            {
                prefab = AssetUtils.Instance.GetAsset("HumanRefinery") as GameObject;
            }
            GameObject refineryGameObj = GameObject.Instantiate(prefab);
            Construct refinery = refineryGameObj.GetComponentInChildren<Construct>();
            refinery.Player = npc;
            refinery.Group = Group.NPC;
            refineryGameObj.SetActive(true);
            refinery.Build();
            return SetConstructPosition(ConstructId.Refinery, LocationOffset.Refinery, refineryGameObj.transform);
        }

        private NodeState CheckRefineryEndCountDown()
        {
            return CheckConstructCountdown(ConstructId.Refinery);
        }
        #endregion

        #region Buy Yard Construct Sequence
        private Sequence BuyYardConstructSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(CheckEnoughGoldToBuyYard),
                new ActionNode(BuyYardConstruct),
                RepeatUntilSetYardLocationSuccess(),
            });
        }

        private Repeater RepeatUntilSetYardLocationSuccess()
        {
            return new Repeater(LocationYardConstructNode());
        }

        private NodeState CheckEnoughGoldToBuyYard()
        {
            return CheckEnoughGold(ConstructPrice.Yard);
        }

        private NodeState BuyYardConstruct()
        {
            return BuyConstruct(ConstructId.Yard);
        }

        #endregion

        #region Unclock Refinery Selector
        private Selector UnClockRefinerySelector()
        {
            return new Selector(new List<BaseNode>()
            {
                new ActionNode(CheckUnclockedRefinery),
                BuyYardConstructSequence(),
            });
        }

        private NodeState CheckUnclockedRefinery()
        {
            return CheckUnclockedConstruct(ConstructId.Refinery);
        }
        #endregion

        #region Buy Refinery Construct Sequence
        private Sequence BuyRefineryConstructSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                UnClockRefinerySelector(),
                new ActionNode(CheckEnoughGoldToBuyRefinery),
                new ActionNode(BuyRefinery),
                new Repeater(LocationRefineryConstructNode()),
            });
        }

        private NodeState CheckEnoughGoldToBuyRefinery()
        {
            return CheckEnoughGold(ConstructPrice.Refinery);
        }

        private NodeState BuyRefinery()
        {
            return BuyConstruct(ConstructId.Refinery);
        }

        #endregion

        #region Select on Refinery Construct
        private Sequence SelectOnRefineryConstruct()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(ChechHasRefinery),
                RepeatUntilEnoughGoldToBuyAgent(),
            });
        }

        private RepeaterUntil RepeatUntilEnoughGoldToBuyAgent()
        {
            return new RepeaterUntil(new ActionNode(SelectOnRefinery), CheckEnoughGoldToBuyAgentAction);
        }

        private NodeState ChechHasRefinery()
        {
            return CheckContainConstruct(typeof(Refinery));
        }

        private NodeState CheckEnoughGoldToBuyAgentAction()
        {
            return CheckEnoughGoldToBuyAgent(typeAgentWantToBuy);
        }

        private NodeState SelectOnRefinery()
        {
            Construct refinery = npc.GetConstruct(typeof(Refinery));
            if (refinery != null && ((Refinery)refinery).IsMax)
            {
                ((Refinery)refinery).Produce(null);
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
        #endregion

        #region Produce Gold Selector
        private Selector ProduceGoldSelector()
        {
            return new Selector(new List<BaseNode>()
            {
                SelectOnRefineryConstruct(),
                BuyRefineryConstructSequence(),
            });
        }
        #endregion

        #region Enough Gold Selector
        private Selector EnoughGoldSelector()
        {
            return new Selector(new List<BaseNode>()
            {
                new ActionNode(CheckEnoughGoldToBuyAgentAction),
                ProduceGoldSelector(),
            });
        }
        #endregion

        #region Buy Agent Sequence
        private Sequence BuyAgentSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(ChooseTypeAgentWantToBuy),
                EnoughGoldSelector(),
                new ActionNode(BuyAgentAction),
            });
        }

        private NodeState BuyAgentAction()
        {
            return BuyAgent(typeAgentWantToBuy);
        }

        private NodeState ChooseTypeAgentWantToBuy()
        {
            int i = Random.Range((int)0, (int)3);
            if (Singleton.classname == "Human")
            {
                typeAgentWantToBuy = OrcAgent[i];
            }
            else
            {
                typeAgentWantToBuy = HumanAgent[i];
            }
            return NodeState.Success;
        }
        #endregion

        #region Wait for all agent queue countdown complete Sequence
        private Sequence WaitAllAgentCountdownSuccessSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
               new ActionNode(CheckAgentQueueFull),
               WaitAllAgentCountdownComplete(),
            });
        }

        private NodeState CheckAgentQueueFull()
        {
            if (agentCountQuery.Count + npc.Agents.Count >= NumAgentToAttack)
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private Repeater WaitAllAgentCountdownComplete()
        {
            return new Repeater(new ActionNode(CheckAllAgentCountdownCompleteAction));
        }

        private NodeState CheckAllAgentCountdownCompleteAction()
        {
            return CheckAllBuyProgressSuccess(typeof(Soldier));
        }
        #endregion

        #region Repeat until enough agent in buy sequence
        private RepeaterUntil RepeatUntilEnoughAgentInSequece()
        {
            return new RepeaterUntil(BuyAgentSequence(), CheckAgentQueueFull);
        }
        #endregion

        #region Produce Agent Sequence
        private Sequence ProduceAgentSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
               HasBarrackSelector(),
               RepeatUntilEnoughAgentInSequece(),
               WaitAllAgentCountdownComplete(),
            });
        }
        #endregion

        #region Has Barrack Selector

        private Selector HasBarrackSelector()
        {
            return new Selector(new List<BaseNode>()
            {
                new ActionNode(CheckHasBarrack),
                CanBuyBarrackSequence(),
            });
        }

        private NodeState CheckHasBarrack()
        {
            return CheckContainConstruct(typeof(Barrack));
        }

        #endregion

        #region Buy Can Barrack Sequence

        private Sequence CanBuyBarrackSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                UnclockBarrackSelector(),
                EnoughGoldToBuyBrrackSelector(),
                new ActionNode(BuyBarrackAction),
                LocationBarrackConstructNode(),
            });
        }

        private Selector EnoughGoldToBuyBrrackSelector()
        {
            return new Selector(new List<BaseNode>()
            {
                new ActionNode(() => CheckEnoughGold(ConstructPrice.Barrack)),
                ProduceGoldSelector(),
            });
        }

        private NodeState BuyBarrackAction()
        {
            return BuyConstruct(ConstructId.Barrack);
        }

        #endregion

        #region Unclock Barrack Selector

        private Selector UnclockBarrackSelector()
        {
            return new Selector(new List<BaseNode>()
            {
                new ActionNode(CheckUnclockedBarrack),
                BuyRefineryConstructSequence(),
            });
        }

        private NodeState CheckUnclockedBarrack()
        {
            return CheckUnclockedConstruct(ConstructId.Barrack);
        }
        #endregion

        #region Location Barrack Sequence

        private NodeState CheckBarrackEndCountdown()
        {
            return CheckConstructCountdown(ConstructId.Barrack);
        }

        private NodeState SetBarrackConstructPosition()
        {
            GameObject prefab;
            if (Singleton.classname == "Human")
            {
                prefab = AssetUtils.Instance.GetAsset("OrcBarrack") as GameObject;
            }
            else
            {
                prefab = AssetUtils.Instance.GetAsset("HumanBarrack") as GameObject;
            }
            GameObject barrackGameObj = GameObject.Instantiate(prefab);
            Construct barrack = barrackGameObj.GetComponentInChildren<Construct>();
            barrack.Player = npc;
            barrack.Group = Group.NPC;
            barrackGameObj.SetActive(true);
            barrack.Build();

            return SetConstructPosition(ConstructId.Barrack, LocationOffset.Barrack, barrackGameObj.transform);
        }

        private Sequence LocationBarrackConstructNode()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(CheckBarrackEndCountdown),
                new ActionNode(SetBarrackConstructPosition),
            });
        }
        #endregion

        #region Enough Agent Selector
        private Selector EnoughAgentSelector()
        {
            return new Selector(new List<BaseNode>()
            {
                WaitAllAgentCountdownSuccessSequence(),
                new ActionNode(CountAgentsToAttack),
                ProduceAgentSequence(),
            });
        }
        #endregion

        #region Can Attack Selector
        private Selector CanAttackSelector()
        {
            return new Selector(new List<BaseNode>()
            {
                new ActionNode(CheckIsAttackAction),
                EnoughAgentSelector(),
            });
        }

        private NodeState CheckIsAttackAction()
        {
            if (attackWaveState == NodeState.Running)
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
        #endregion

        #region Group Attack Wave Sequence
        private Sequence GroupAttackSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(MoveAgentsToWayPoint),
                new ActionNode(CheckAgentsReachedTarget),
                KillAllOrDeadAllRepeater(),

            });
        }

        private NodeState MoveAgentsToWayPoint()
        {
            if (CheckAgentsReachedTarget() == NodeState.Failure) return NodeState.Failure;
            if (attackWaveState != NodeState.Running)
            {
                attackWaveState = NodeState.Running;
                if (currentTarget == null)
                    return MoveAgentsTo(WayPoint, TargetType.Place, null);
                else
                    return MoveAgentsTo(currentTarget.Position, TargetType.NPC, currentTarget);
            }
            return NodeState.Success;
        }

        #endregion

        #region Clear All Unit progress
        private Sequence ClearAllUnitProgressSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                CanAttackSelector(),
                ReqairAttackProgressSequence(),
            });
        }
        #endregion

        #region Reqair Attack Progress Sequence
        private Sequence ReqairAttackProgressSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                GroupAttackSequence(),
            });
        }

        private NodeState ResetAttackStateAction()
        {
            if (!isAttack)
            {
                isAttack = true;
                return NodeState.Success;
            }
            else
            {
                if (npc.Agents.Count <= 0)
                {
                    isAttack = false;
                    return NodeState.Failure;
                }
                return NodeState.Success;
            }
        }

        #region Repeat until kill all or dead all
        private RepeaterUntil KillAllOrDeadAllRepeater()
        {
            return new RepeaterUntil(AttackSequence(), CheckAttackWaveTerminal);
        }

        private NodeState CheckAttackWaveTerminal()
        {
            if (!mainPlayer.IsAlive() || !npc.IsAlive() || npc.Agents.Count <= 0)
            {
                attackWaveState = NodeState.Failure;
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        private Sequence AttackSequence()
        {
            return new Sequence(new List<BaseNode>()
            {
                new ActionNode(ChooseTargetAction),
                new ActionNode(SetTargetAction),
            });
        }

        private NodeState ChooseTargetAction()
        {
            List<AIAgent> agents = npc.Agents;
            if (agents.Count <= 0) return NodeState.Failure;
            if (currentTarget == null || currentTarget.IsDead)
            {
                Vector3 averagePos = agents[0].Position;
                for (int i = 1; i < agents.Count; i++)
                {
                    averagePos += agents[i].Position;
                }
                averagePos /= agents.Count;

                if (mainPlayer.Constructs.Count > 0)
                {
                    List<Construct> playerCons = mainPlayer.Constructs;
                    currentTarget = playerCons[0];
                    float minDis = Vector3.Distance(currentTarget.Position, averagePos);
                    for (int i = 1; i < playerCons.Count; i++)
                    {
                        float dist = Vector3.Distance(playerCons[i].Position, averagePos);
                        if(minDis > dist)
                        {
                            minDis = dist;
                            currentTarget = playerCons[i];
                        }
                    }
                }
                else
                {
                    if (mainPlayer.Agents.Count > 0)
                    {
                        agents = mainPlayer.Agents;
                        currentTarget = mainPlayer.Agents[0];
                        float minDis = Vector3.Distance(currentTarget.Position, averagePos);
                        for (int i = 1; i < agents.Count; i++)
                        {
                            float dist = Vector3.Distance(agents[i].Position, averagePos);
                            if (minDis > dist)
                            {
                                minDis = dist;
                                currentTarget = agents[i];
                            }
                        }
                    }
                }
            }
            return NodeState.Success;
        }

        private NodeState SetTargetAction()
        {
            if (currentTarget != null && !currentTarget.IsDead)
            {
                return MoveAgentsTo(currentTarget.Position, TargetType.NPC, currentTarget);
            }
            return NodeState.Success;
        }
        #endregion
        #endregion
    }
}