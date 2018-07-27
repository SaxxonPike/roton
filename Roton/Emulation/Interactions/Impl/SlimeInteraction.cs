using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x25)]
    [Context(Context.Super, 0x25)]
    public sealed class SlimeInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public SlimeInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = Engine.Tiles[location].Color;
            var slimeIndex = Engine.ActorIndexAt(location);
            Engine.Harm(slimeIndex);
            Engine.Tiles[location].SetTo(Engine.ElementList.BreakableId, color);
            Engine.UpdateBoard(location);
            Engine.PlaySound(2, Engine.Sounds.SlimeDie);
        }
    }
}