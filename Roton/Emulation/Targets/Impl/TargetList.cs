using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class TargetList : ITargetList
    {
        private readonly Lazy<IDictionary<string, ITarget>> _targets;

        public TargetList(Lazy<IContextMetadataService> contextMetadataService, Lazy<IEnumerable<ITarget>> targets)
        {
            _targets = new Lazy<IDictionary<string, ITarget>>(() =>
            {
                var result = new Dictionary<string, ITarget>();
                foreach (var target in targets.Value)
                {
                    foreach (var attribute in contextMetadataService.Value.GetMetadata(target))
                        result.Add(attribute.Name, target);
                }

                return result;
            });
        }
        
        public ITarget Get(string name)
        {
            return _targets.Value.ContainsKey(name)
                ? _targets.Value[name]
                : null;
        }        
    }
}