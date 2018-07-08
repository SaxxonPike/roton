﻿using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class DragonPupBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.DragonPup;

        public DragonPupBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            _engine.UpdateBoard(_engine.Actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch (_engine.State.GameCycle & 0x3)
            {
                case 0:
                case 2:
                    return new AnsiChar(0x94, _engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xA2, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x95, _engine.Tiles[location].Color);
            }
        }
    }
}