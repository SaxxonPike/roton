using Roton.Core;

namespace Roton.Emulation.Items
{
    public class TimeItem : IItem
    {
        private readonly IEngine _engine;

        public TimeItem(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "TIME";

        public int Value
        {
            get => _engine.World.TimePassed;
            set => _engine.World.TimePassed = value;
        }
    }
}