using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class ConditionList : IConditionList
    {
        private readonly Lazy<IDictionary<string, ICondition>> _conditions;

        public ConditionList(Lazy<IContextMetadataService> contextMetadataService, Lazy<IEnumerable<ICondition>> conditions)
        {
            _conditions = new Lazy<IDictionary<string, ICondition>>(() =>
            {
                var result = new Dictionary<string, ICondition>();
                foreach (var condition in conditions.Value)
                {
                    foreach (var attribute in contextMetadataService.Value.GetMetadata(condition))
                        result.Add(attribute.Name, condition);
                }

                return result;
            });
        }
        
        public ICondition Get(string name)
        {
            return _conditions.Value.ContainsKey(name)
                ? _conditions.Value[name]
                : null;
        }        
    }
}