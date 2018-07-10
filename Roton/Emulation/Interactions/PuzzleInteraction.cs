using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Interactions
{
    public class PuzzleInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public PuzzleInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Push(location, vector);
            _engine.PlaySound(2, _engine.Sounds.Push);
        }
    }
}