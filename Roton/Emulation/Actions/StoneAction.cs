using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Super, 0x40)]
    public sealed class StoneAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public StoneAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            Engine.Tiles[actor.Location].Color =
                (Engine.Tiles[actor.Location].Color & 0x70) + Engine.Random.GetNext(7) + 9;
            Engine.UpdateBoard(Engine.Actors[index].Location);
        }
    }
}