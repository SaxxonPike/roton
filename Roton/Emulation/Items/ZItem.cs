using Roton.Core;

namespace Roton.Emulation.Items
{
    public class ZItem : IItem
    {
        private readonly IEngine _engine;

        public ZItem(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "Z";

        public int Value
        {
            get => _engine.World.Stones;
            set => _engine.World.Stones = value;
        }
    }
}