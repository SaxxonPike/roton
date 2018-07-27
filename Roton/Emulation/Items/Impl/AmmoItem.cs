using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl
{
    [Context(Context.Original, "AMMO")]
    [Context(Context.Super, "AMMO")]
    public sealed class AmmoItem : IItem
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public AmmoItem(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => Engine.World.Ammo;
            set => Engine.World.Ammo = value;
        }
    }
}