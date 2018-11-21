using EnumCollection;

namespace AIs.BT.Commoms
{
    public class Inverter : BaseNode
    {
        protected BaseNode child;

        public Inverter(BaseNode argChild) : base()
        {
            child = argChild;
        }

        public override NodeState Evaluate()
        {          
            switch(child.Evaluate())
            {
                case NodeState.Failure:
                    State = NodeState.Success;
                    break;
                case NodeState.Success:
                    State = NodeState.Failure;
                    break;
                case NodeState.Running:
                    State = NodeState.Running;
                    break;
            }
            return State;
        }
    }
}