using UnityEngine;

namespace WSP.Units
{
    public class ActionContext
    {
        public IAction Action { get; }
        public Vector2Int Target { get; }
        public bool ActionStarted => Action?.ActionStarted ?? false;

        public ActionContext(IAction action, Vector2Int target)
        {
            Action = action;
            Target = target;
        }
    }
}