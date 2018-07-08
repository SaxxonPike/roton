﻿using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public abstract class PuzzleBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public PuzzleBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Push(location, vector);
            _engine.PlaySound(2, _engine.Sounds.Push);
        }
    }
}