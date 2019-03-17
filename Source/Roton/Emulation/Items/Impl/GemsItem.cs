using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl
{
    [Context(Context.Original, "GEMS")]
    [Context(Context.Super, "GEMS")]
    public sealed class GemsItem : IItem
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public GemsItem(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => Engine.World.Gems;
            set => Engine.World.Gems = value;
        }
    }
}