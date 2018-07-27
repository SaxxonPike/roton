using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl
{
    [Context(Context.Original, 0x0C)]
    [Context(Context.Super, 0x0C)]
    public sealed class DuplicatorDraw : IDraw
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public DuplicatorDraw(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            switch (Engine.ActorAt(location).P1)
            {
                case 2:
                    return new AnsiChar(0xF9, Engine.Tiles[location].Color);
                case 3:
                    return new AnsiChar(0xF8, Engine.Tiles[location].Color);
                case 4:
                    return new AnsiChar(0x6F, Engine.Tiles[location].Color);
                case 5:
                    return new AnsiChar(0x4F, Engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0xFA, Engine.Tiles[location].Color);
            }
        }
    }
}