using System;
using System.Collections.Generic;
using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztGrammar : Grammar
    {
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

        public SuperZztGrammar(
            IColors colors, IElements elements, IWorld world, IBoard board, IEngine engine, ISounds sounds, IGrid grid,
            IActors actors, IFlags flags, IState state, IRandom random)
            : base(colors, elements, world, board, engine, sounds, grid, actors, flags, state, random)
        {
        }
    }
}