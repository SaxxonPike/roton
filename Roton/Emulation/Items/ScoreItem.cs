using Roton.Core;

namespace Roton.Emulation.Items
{
    public class ScoreItem : IItem
    {
        private readonly IEngine _engine;

        public ScoreItem(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "SCORE";

        public int Value
        {
            get => _engine.World.Score;
            set => _engine.World.Score = value;
        }
    }
}