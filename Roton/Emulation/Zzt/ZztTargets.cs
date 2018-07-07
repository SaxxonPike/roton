using System.Collections.Generic;
using Roton.Emulation.Targets;

namespace Roton.Emulation.SuperZZT
{
    public class ZztTargets : Targets
    {
        public ZztTargets(IEnumerable<ITarget> items) : base(items, new string[]{})
        {
        }
    }
}