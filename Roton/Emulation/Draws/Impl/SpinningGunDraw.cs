using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl
{
    [Context(Context.Original, 0x27)]
    [Context(Context.Super, 0x27)]
    public sealed class SpinningGunDraw : IDraw
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public SpinningGunDraw(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            switch (Engine.State.GameCycle & 0x7)
            {
                case 0:
                case 1:
                    return new AnsiChar(0x18, Engine.Tiles[location].Color);
                case 2:
                case 3:
                    return new AnsiChar(0x1A, Engine.Tiles[location].Color);
                case 4:
                case 5:
                    return new AnsiChar(0x19, Engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x1B, Engine.Tiles[location].Color);
            }
        }
    }
}