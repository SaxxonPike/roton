using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterAction(Lazy<IEngine> engine) : IAction
{
    private IEngine Engine => engine.Value;

    public void Act(int index)
    {
        Engine.UpdateBoard(Engine.Actors[index].Location);
    }
}