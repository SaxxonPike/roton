using System.Collections.Generic;

namespace Roton.Emulation.Targets
{
    public abstract class TargetList : ITargetList
    {
        private readonly IDictionary<string, ITarget> _targets;

        protected TargetList(IDictionary<string, ITarget> targets)
        {
            _targets = targets;
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