using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl
{
    [Context(Context.Super, 0x40)]
    public sealed class StoneDraw : IDraw
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public StoneDraw(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x41 + Engine.Random.GetNext(0x1A), Engine.Tiles[location].Color);
        }
    }
}