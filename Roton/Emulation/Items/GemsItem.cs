using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Items
{
    public class GemsItem : IItem
    {
        private readonly IEngine _engine;

        public GemsItem(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "GEMS";

        public int Value
        {
            get => _engine.World.Gems;
            set => _engine.World.Gems = value;
        }
    }
}