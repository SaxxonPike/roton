using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [ContextEngine(ContextEngine.Original, 0x1E)]
    [ContextEngine(ContextEngine.Super, 0x1E)]
    public sealed class TransporterAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public TransporterAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            Engine.UpdateBoard(Engine.Actors[index].Location);
        }
    }
}