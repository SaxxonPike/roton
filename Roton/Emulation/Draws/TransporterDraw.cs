﻿using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Draws
{
    public class TransporterDraw : IDraw
    {
        private readonly IEngine _engine;

        public TransporterDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            var actor = _engine.ActorAt(location);

            var index = actor.Cycle > 0 
                ? (_engine.State.GameCycle / actor.Cycle) & 0x3 
                : 0;
                
            if (actor.Vector.X == 0)
            {
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(_engine.State.TransporterVChars[index], _engine.Tiles[location].Color);
            }

            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(_engine.State.TransporterHChars[index], _engine.Tiles[location].Color);
        }
    }
}