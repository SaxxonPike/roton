using System.Collections.Generic;
using Roton.Emulation.Conditions;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztConditions : Conditions.Conditions
    {
        public SuperZztConditions(IEnumerable<ICondition> commands) : base(commands, new string[]{})
        {
        }
    }
}