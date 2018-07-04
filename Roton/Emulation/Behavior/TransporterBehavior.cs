﻿using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class TransporterBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly IGrid _grid;
        
        public override string KnownName => KnownNames.Transporter;

        public TransporterBehavior(IEngine engine, IActors actors, IState state, IGrid grid)
        {
            _engine = engine;
            _actors = actors;
            _state = state;
            _grid = grid;
        }
        
        public override void Act(int index)
        {
            _engine.UpdateBoard(_actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var actor = _actors.ActorAt(location);
            int index;

            if (actor.Vector.X == 0)
            {
                if (actor.Cycle > 0)
                    index = (_state.GameCycle/actor.Cycle) & 0x3;
                else
                    index = 0;
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(_state.TransporterVChars[index], _grid[location].Color);
            }
            if (actor.Cycle > 0)
                index = (_state.GameCycle/actor.Cycle) & 0x3;
            else
                index = 0;
            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(_state.TransporterHChars[index], _grid[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }
    }
}