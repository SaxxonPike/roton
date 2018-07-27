using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class TargetList : ITargetList
    {
        private readonly IDictionary<string, ITarget> _targets = new Dictionary<string, ITarget>();

        public TargetList(IContextMetadataService contextMetadataService, IEnumerable<ITarget> targets)
        {
            foreach (var target in targets)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(target))
                    _targets.Add(attribute.Name, target);
            }
        }
        
        public ITarget Get(string name)
        {
            return _targets.ContainsKey(name)
                ? _targets[name]
                : null;
        }        
    }
}