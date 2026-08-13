using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "ENERGIZED")]
[Context(Context.Super, "ENERGIZED")]
public sealed class EnergizedCondition(Lazy<IEngine> engine) : ICondition
{
    private IEngine Engine => engine.Value;

    public bool? Execute(IOopContext context)
    {
        return Engine.World.EnergyCycles > 0;
    }
}