﻿using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x1E)]
    [ContextEngine(ContextEngine.SuperZzt, 0x1E)]
    public sealed class TransporterInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public TransporterInteraction(IEngine engine)
        {
            _engine = engine;
        }

        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }
    }
}