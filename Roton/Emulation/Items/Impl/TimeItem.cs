using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl
{
    [Context(Context.Original, "TIME")]
    [Context(Context.Super, "TIME")]
    public sealed class TimeItem : IItem
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public TimeItem(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => Engine.World.TimePassed;
            set => Engine.World.TimePassed = value;
        }
    }
}