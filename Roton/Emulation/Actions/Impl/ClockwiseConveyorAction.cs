using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x10)]
    [Context(Context.Super, 0x10)]
    public sealed class ClockwiseConveyorAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public ClockwiseConveyorAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            Engine.UpdateBoard(actor.Location);
            Engine.Convey(actor.Location, 1);
        }
    }
}