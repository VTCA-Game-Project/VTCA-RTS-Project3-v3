using DelegateCollection;
using EnumCollection;

namespace AIs.BT.Commoms
{
    public class RepeaterUntil : BaseNode
    {
        protected BaseNode child;
        protected ActionNodeDelegate terminalCondition;

        public RepeaterUntil(BaseNode argChild,ActionNodeDelegate condition) : base()
        {
            child = argChild;
            terminalCondition = condition;
        }

        public override NodeState Evaluate()
        {
            switch (terminalCondition())
            {
                case NodeState.Failure:
                case NodeState.Running:
                    child.Evaluate();
                    State = NodeState.Running;
                    return State;
                case NodeState.Success:
                    State = NodeState.Success;
                    return State;
            }
            return State;
        }
    }
}