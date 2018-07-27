using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl
{
    [Context(Context.Original, "TIME")]
    [Context(Context.Super, "TIME")]
    public sealed class TimeCheat : ICheat
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public TimeCheat(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(string name, bool clear)
        {
            Engine.World.TimePassed -= 30;
        }
    }
}