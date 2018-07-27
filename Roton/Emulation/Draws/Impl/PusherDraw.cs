using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl
{
    [Context(Context.Original, 0x28)]
    [Context(Context.Super, 0x28)]
    public sealed class PusherDraw : IDraw
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public PusherDraw(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            var actor = Engine.ActorAt(location);
            switch (actor.Vector.X)
            {
                case 1:
                    return new AnsiChar(0x10, Engine.Tiles[location].Color);
                case -1:
                    return new AnsiChar(0x11, Engine.Tiles[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, Engine.Tiles[location].Color)
                        : new AnsiChar(0x1F, Engine.Tiles[location].Color);
            }
        }
    }
}