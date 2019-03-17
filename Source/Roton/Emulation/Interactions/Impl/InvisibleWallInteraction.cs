using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x1C)]
    [Context(Context.Super, 0x1C)]
    public sealed class InvisibleWallInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public InvisibleWallInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            Engine.Tiles[location].Id = Engine.ElementList.NormalId;
            Engine.UpdateBoard(location);
            Engine.PlaySound(3, Engine.Sounds.Invisible);
            Engine.SetMessage(0x64, Engine.Alerts.InvisibleMessage);
        }
    }
}