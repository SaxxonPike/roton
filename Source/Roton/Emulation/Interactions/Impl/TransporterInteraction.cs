using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterInteraction(Lazy<IEngine> engine) : IInteraction
{
    private IEngine Engine => engine.Value;

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        Engine.PushThroughTransporter(location.Difference(vector), vector);
        vector.SetTo(0, 0);
    }
}