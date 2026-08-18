using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BIND")]
[Context(Context.Super, "BIND")]
public sealed class BindCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[256];

        var search = new SearchContext();
        var target = Engine.Parser.ReadWord(context.Index, ref instruction, buffer);
        if (Engine.Parser.GetTarget(context.Index, ref search, target))
        {
            var targetActor = Engine.Actors[search.Index];
            context.Actor.Pointer = targetActor.Pointer;
            context.Actor.Length = targetActor.Length;
            instruction = 0;
        }
    }
}