using DelegateCollection;
using EnumCollection;

namespace AIs.BT.Commoms
{
    public class ActionNode : BaseNode
    {
        protected ActionNodeDelegate action;

        public ActionNode(ActionNodeDelegate argAction) : base()
        {
            action = argAction;
        }

        public override NodeState Evaluate()
        {
            State = action();
            return State;
        }
    }
}