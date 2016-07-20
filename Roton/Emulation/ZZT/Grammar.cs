using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    public class Grammar : GrammarBase
    {
        public Grammar(IColorList colors, IElementList elements) : base(colors, elements)
        {
        }

        protected override IDictionary<string, Action<ICore>> GetCheats()
        {
            return new Dictionary<string, Action<ICore>>
            {
                { "AMMO", Cheat_Ammo },
                { "DARK", Cheat_Dark },
                { "GEMS", Cheat_Gems },
                { "HEALTH", Cheat_Health },
                { "KEYS", Cheat_Keys },
                { "TIME", Cheat_Time },
                { "TORCHES", Cheat_Torches },
                { "ZAP", Cheat_Zap }
            };
        }

        protected override IDictionary<string, Action<IOopContext>> GetCommands()
        {
            throw new NotImplementedException();
        }

        protected override IDictionary<string, Func<IOopContext, bool?>> GetConditions()
        {
            return new Dictionary<string, Func<IOopContext, bool?>>
            {
                { "ALLIGNED", Condition_Alligned },
                { "ANY", Condition_Any },
                { "BLOCKED", Condition_Blocked },
                { "CONTACT", Condition_Contact },
                { "ENERGIZED", Condition_Energized },
                { "NOT", Condition_Not }
            };
        }

        protected override IDictionary<string, Func<IOopContext, IXyPair>> GetDirections()
        {
            return new Dictionary<string, Func<IOopContext, IXyPair>>
            {
                { "CW", Direction_Cw },
                { "CCW", Direction_Ccw },
                { "E", Direction_East },
                { "EAST", Direction_East },
                { "FLOW", Direction_Flow },
                { "I", Direction_Idle },
                { "IDLE", Direction_Idle },
                { "N", Direction_North },
                { "NORTH", Direction_North },
                { "OPP", Direction_Opp },
                { "RND", Direction_Rnd },
                { "RNDNE", Direction_RndNe },
                { "RNDNS", Direction_RndNs },
                { "RNDP", Direction_RndP },
                { "S", Direction_South },
                { "SOUTH", Direction_South },
                { "SEEK", Direction_Seek },
                { "W", Direction_West },
                { "WEST", Direction_West }
            };
        }

        protected override IDictionary<string, Func<IOopContext, IOopItem>> GetItems()
        {
            return new Dictionary<string, Func<IOopContext, IOopItem>>
            {
                { "AMMO", Item_Ammo },
                { "GEMS", Item_Gems },
                { "HEALTH", Item_Health },
                { "SCORE", Item_Score },
                { "TIME", Item_Time },
                { "TORCHES", Item_Torches }
            };
        }
    }
}
