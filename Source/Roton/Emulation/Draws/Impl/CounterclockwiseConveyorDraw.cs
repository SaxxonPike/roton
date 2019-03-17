using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl
{
    [Context(Context.Original, 0x11)]
    [Context(Context.Super, 0x11)]
    public sealed class CounterclockwiseConveyorDraw : IDraw
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public CounterclockwiseConveyorDraw(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            switch ((Engine.State.GameCycle / Engine.ElementList[Engine.ElementList.CounterId].Cycle) & 0x3)
            {
                case 3:
                    return new AnsiChar(0xB3, Engine.Tiles[location].Color);
                case 2:
                    return new AnsiChar(0x2F, Engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xC4, Engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, Engine.Tiles[location].Color);
            }
        }
    }
}