using System.Collections.Generic;
using Roton.Emulation.Targets;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztTargets : Targets.Targets
    {
        public SuperZztTargets(IEnumerable<ITarget> items) : base(items, new string[]{})
        {
        }
    }
}