using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x0A)]
    [ContextEngine(ContextEngine.SuperZzt, 0x0A)]
    public sealed class ScrollAction : IAction
    {
        private readonly IEngine _engine;

        public ScrollAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var color = _engine.Tiles[actor.Location].Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            _engine.Tiles[actor.Location].Color = color;
            _engine.UpdateBoard(actor.Location);
        }
    }
}