﻿using Roton.Emulation.Core;

namespace Roton.Emulation.Actions
{
    public class TransporterAction : IAction
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