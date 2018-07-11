using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Zzt, 0x11)]
    [ContextEngine(ContextEngine.SuperZzt, 0x11)]
    public sealed class CounterclockwiseConveyorDraw : IDraw
    {
        private readonly IEngine _engine;

        public CounterclockwiseConveyorDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            switch ((_engine.State.GameCycle / _engine.ElementList[_engine.ElementList.CounterId].Cycle) & 0x3)
            {
                case 3:
                    return new AnsiChar(0xB3, _engine.Tiles[location].Color);
                case 2:
                    return new AnsiChar(0x2F, _engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xC4, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, _engine.Tiles[location].Color);
            }
        }
    }
}