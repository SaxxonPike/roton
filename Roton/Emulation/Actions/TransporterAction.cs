﻿using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x1E)]
    [ContextEngine(ContextEngine.Super, 0x1E)]
    public sealed class TransporterAction : IAction
    {
        private readonly IEngine _engine;

        public TransporterAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            _engine.UpdateBoard(_engine.Actors[index].Location);
        }
    }
}