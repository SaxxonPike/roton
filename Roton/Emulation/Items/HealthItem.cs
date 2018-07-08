using Roton.Core;

namespace Roton.Emulation.Items
{
    public class HealthItem : IItem
    {
        private readonly IEngine _engine;

        public HealthItem(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "HEALTH";

        public int Value
        {
            get => _engine.World.Health;
            set => _engine.World.Health = value;
        }
    }
}