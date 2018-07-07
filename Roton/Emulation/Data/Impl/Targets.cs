using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Targets;

namespace Roton.Emulation.SuperZZT
{
    public abstract class Targets : ITargets
    {
        private readonly IDictionary<string, ITarget> _targets;

        protected Targets(IEnumerable<ITarget> items, string[] enabledNames)
        {
            _targets = items
                .Where(c => enabledNames.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c);
        }
        
        public ITarget Get(string name)
        {
            return _targets.ContainsKey(name)
                ? _targets[name]
                : null;
        }

        public ITarget GetDefault() => Get(string.Empty);
    }
}