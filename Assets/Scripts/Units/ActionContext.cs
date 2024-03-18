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

        public void StartAction(IUnit unit)
        {
            Action.StartAction(unit, Target);
        }
    }
}