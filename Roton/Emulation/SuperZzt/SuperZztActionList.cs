using System.Collections.Generic;
using Roton.Emulation.Actions;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztActionList : ActionList
    {
        public SuperZztActionList(IDictionary<int, IAction> actions, IAction defaultAction) : base(actions, defaultAction)
        {
        }
    }
}