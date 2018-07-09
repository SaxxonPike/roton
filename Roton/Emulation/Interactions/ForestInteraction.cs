using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.Behaviors
{
    public class ForestInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public ForestInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.ClearForest(location);
            _engine.UpdateBoard(location);

            var forestIndex = _engine.State.ForestIndex;
            var forestSongLength = _engine.Sounds.Forest.Length;
            _engine.State.ForestIndex = (forestIndex + 2) % forestSongLength;
            _engine.PlaySound(3, _engine.Sounds.Forest, forestIndex, 2);

            if (!_engine.Alerts.Forest)
                return;

            _engine.SetMessage(0xC8, _engine.Alerts.ForestMessage);
            _engine.Alerts.Forest = false;
        }
    }
}