using System.Collections.Generic;
using Roton.Emulation.Conditions;

namespace Roton.Emulation.SuperZZT
{
    public class ZztConditions : Conditions.Conditions
    {
        public ZztConditions(IEnumerable<ICondition> commands) : base(commands, new string[]{})
        {
        }
    }
}