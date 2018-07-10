using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Draws
{
    public class DuplicatorDraw : IDraw
    {
        private readonly IEngine _engine;

        public DuplicatorDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            switch (_engine.ActorAt(location).P1)
            {
                case 2:
                    return new AnsiChar(0xF9, _engine.Tiles[location].Color);
                case 3:
                    return new AnsiChar(0xF8, _engine.Tiles[location].Color);
                case 4:
                    return new AnsiChar(0x6F, _engine.Tiles[location].Color);
                case 5:
                    return new AnsiChar(0x4F, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0xFA, _engine.Tiles[location].Color);
            }
        }
    }
}