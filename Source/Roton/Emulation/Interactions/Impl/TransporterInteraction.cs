using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterInteraction : IInteraction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public TransporterInteraction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        Engine.PushThroughTransporter(location.Difference(vector), vector);
        vector.SetTo(0, 0);
    }
}