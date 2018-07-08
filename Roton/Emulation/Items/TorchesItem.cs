using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Items
{
    public class TorchesItem : IItem
    {
        private readonly IEngine _engine;

        public TorchesItem(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "TORCHES";

        public int Value
        {
            get => _engine.World.Torches;
            set => _engine.World.Torches = value;
        }
    }
}