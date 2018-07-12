using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Original, "SCORE")]
    [ContextEngine(ContextEngine.Super, "SCORE")]
    public sealed class ScoreItem : IItem
    {
        private readonly IEngine _engine;

        public ScoreItem(IEngine engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => _engine.World.Score;
            set => _engine.World.Score = value;
        }
    }
}