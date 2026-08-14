using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Super, 0x3C)]
public sealed class DragonPupAction(Lazy<IEngine> engine) : IAction
{
    private IEngine Engine => engine.Value;

    public void Act(int index)
    {
        Engine.UpdateBoard(Engine.Actors[index].Location);
    }
}