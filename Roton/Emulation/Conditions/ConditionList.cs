using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class ConditionList : IConditionList
    {
        private readonly IDictionary<string, ICondition> _conditions = new Dictionary<string, ICondition>();

        public ConditionList(IContextMetadataService contextMetadataService, IEnumerable<ICondition> conditions)
        {
            foreach (var condition in conditions)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(condition))
                    _conditions.Add(attribute.Name, condition);
            }
        }
        
        public ICondition Get(string name)
        {
            return _conditions.ContainsKey(name)
                ? _conditions[name]
                : null;
        }        
    }
}