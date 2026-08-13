using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "NOT")]
[Context(Context.Super, "NOT")]
public sealed class NotCondition(Lazy<IEngine> engine) : ICondition
{
    private IEngine Engine => engine.Value;

    public bool? Execute(IOopContext context)
    {
        return !Engine.Parser.GetCondition(context);
    }
}