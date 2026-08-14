using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "THROWSTAR")]
[Context(Context.Super, "THROWSTAR")]
public sealed class ThrowstarCommand(Lazy<IEngine> engine) : ICommand
{
    private IEngine Engine => engine.Value;

    public void Execute(IOopContext context)
    {
        var vector = Engine.Parser.GetDirection(context);
        if (vector != null)
        {
            var projectile = Engine.ElementList.Star();
            Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
        }
        context.Moved = true;
    }
}