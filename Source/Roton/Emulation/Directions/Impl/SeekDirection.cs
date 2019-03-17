using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl
{
    [Context(Context.Original, "SEEK")]
    [Context(Context.Super, "SEEK")]
    public sealed class SeekDirection : IDirection
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public SeekDirection(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public IXyPair Execute(IOopContext context)
        {
            return Engine.Seek(context.Actor.Location);
        }
    }
}