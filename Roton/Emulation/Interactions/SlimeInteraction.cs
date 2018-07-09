using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.Behaviors
{
    public class SlimeInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public SlimeInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = _engine.Tiles[location].Color;
            var slimeIndex = _engine.ActorIndexAt(location);
            _engine.Harm(slimeIndex);
            _engine.Tiles[location].SetTo(_engine.ElementList.BreakableId, color);
            _engine.UpdateBoard(location);
            _engine.PlaySound(2, _engine.Sounds.SlimeDie);
        }
    }
}