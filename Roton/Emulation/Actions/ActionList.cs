using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class ActionList : IActionList
    {
        private readonly IDictionary<int, IAction> _actions = new Dictionary<int, IAction>();

        public ActionList(IContextMetadataService contextMetadataService, IEnumerable<IAction> actions)
        {
            foreach (var action in actions)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(action))
                    _actions.Add(attribute.Id, action);
            }
        }
        
        public IAction Get(int index)
        {
            return _actions.ContainsKey(index) ? _actions[index] : _actions[-1];
        }

    }
}