using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztGrammar : Grammar
    {
        public SuperZztGrammar(IColorList colors, IElementList elements) : base(colors, elements)
        {
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
