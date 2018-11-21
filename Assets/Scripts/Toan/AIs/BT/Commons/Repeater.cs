using DelegateCollection;
using EnumCollection;

namespace AIs.BT.Commoms
{
    public class Repeater : BaseNode
    {
        protected BaseNode child;

        public Repeater(BaseNode argChild) : base()
        {
            child = argChild;
        }

        public override NodeState Evaluate()
        {            
            switch (child.Evaluate())
            {
                case NodeState.Failure:
                case NodeState.Running:
                    State = NodeState.Running;
                    break;
                case NodeState.Success:
                    State = NodeState.Success;
                    return State;
            }
            return State;
        }
    }
}