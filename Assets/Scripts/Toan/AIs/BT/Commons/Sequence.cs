using System.Collections.Generic;
using EnumCollection;

namespace AIs.BT.Commoms
{
    public class Sequence : BaseNode
    {
        protected List<BaseNode> children;

        public Sequence(List<BaseNode> argChildren) : base()
        {
            children = argChildren;
        }

        public override NodeState Evaluate()
        {
            for (int i = 0; i < children.Count; i++)
            {
                switch (children[i].Evaluate())
                {
                    case NodeState.Failure:
                        State = NodeState.Failure;
                        return State;
                    case NodeState.Success:
                        continue;                        
                    case NodeState.Running:
                        State = NodeState.Running;
                        return State;
                }
            }
            State = NodeState.Success;
            return State;
        }
    }
}