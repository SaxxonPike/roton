using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x13)]
    [Context(Context.Super, 0x13)]
    public sealed class WaterInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;
        
        public WaterInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            Engine.PlaySound(3, Engine.Sounds.Water);
            Engine.SetMessage(0x64, Engine.Alerts.WaterMessage);
        }
    }
}