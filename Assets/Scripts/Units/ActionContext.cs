namespace WSP.Units
{
    public class ActionContext
    {
        public IAction Action { get; }
        public ActionTarget Target { get; }
        public bool ActionStarted => Action?.ActionStarted ?? false;

        public ActionContext(IAction action, ActionTarget target)
        {
            Action = action;
            Target = target;
        }

        public bool StartAction(IUnit unit)
        {
            return Action.StartAction(unit, Target);
        }
    }
}