using System;
using System.Collections.Generic;
using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztGrammar : Grammar
    {
        public SuperZztGrammar(IColorList colors, IElementList elementList) : base(colors, elementList)
        {
        }

        protected override void Command_Lock(IOopContext context)
        {
            context.Actor.P3 = 1;
        }

        protected override void Command_Unlock(IOopContext context)
        {
            context.Actor.P3 = 0;
        }

        protected override IDictionary<string, Func<IOopContext, IOopItem>> GetItems()
        {
            return new Dictionary<string, Func<IOopContext, IOopItem>>
            {
                {"AMMO", Item_Ammo},
                {"GEMS", Item_Gems},
                {"HEALTH", Item_Health},
                {"SCORE", Item_Score},
                {"TIME", Item_Time},
                {"Z", Item_Stones}
            };
        }
    }
}