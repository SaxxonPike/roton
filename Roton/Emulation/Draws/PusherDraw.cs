using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Original, 0x28)]
    [ContextEngine(ContextEngine.Super, 0x28)]
    public sealed class PusherDraw : IDraw
    {
        private readonly IEngine _engine;

        public PusherDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            var actor = _engine.ActorAt(location);
            switch (actor.Vector.X)
            {
                case 1:
                    return new AnsiChar(0x10, _engine.Tiles[location].Color);
                case -1:
                    return new AnsiChar(0x11, _engine.Tiles[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, _engine.Tiles[location].Color)
                        : new AnsiChar(0x1F, _engine.Tiles[location].Color);
            }
        }
    }
}