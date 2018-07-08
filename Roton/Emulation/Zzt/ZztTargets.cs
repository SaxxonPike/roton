using System.Collections.Generic;
using Roton.Emulation.Targets;

namespace Roton.Emulation.Zzt
{
    public class ZztTargets : Targets.Targets
    {
        public ZztTargets(IEnumerable<ITarget> items) : base(items, new string[]{})
        {
        }
    }
}