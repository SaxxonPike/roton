using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class ActionList : IActionList
    {
        private readonly Lazy<IDictionary<int, IAction>> _actions;
        private IDictionary<int, IAction> Actions => _actions.Value;

        public ActionList(Lazy<IContextMetadataService> contextMetadataService, Lazy<IEnumerable<IAction>> actions)
        {
            _actions = new Lazy<IDictionary<int, IAction>>(() =>
            {
                var result = new Dictionary<int, IAction>();
                foreach (var action in actions.Value)
                {
                    foreach (var attribute in contextMetadataService.Value.GetMetadata(action))
                        result.Add(attribute.Id, action);
                }

                return result;
            });
        }
        
        public IAction Get(int index)
        {
            return Actions.ContainsKey(index) ? Actions[index] : Actions[-1];
        }

    }
}