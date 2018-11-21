using EnumCollection;
using System.Collections;
using System.Collections.Generic;

namespace AIs.BT.Commoms
{
    public abstract class BaseNode
    {
        public string NodeName { get; set; }
        public NodeState State { get; protected set; }

        public abstract NodeState Evaluate();
        public BaseNode() { State = NodeState.Running; }
    }
}