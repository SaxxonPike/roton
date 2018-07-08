using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Items
{
    public class AmmoItem : IItem
    {
        private readonly IEngine _engine;

        public AmmoItem(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "AMMO";

        public int Value
        {
            get => _engine.World.Ammo;
            set => _engine.World.Ammo = value;
        }
    }
}