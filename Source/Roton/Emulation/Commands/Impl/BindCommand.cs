using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BIND")]
[Context(Context.Super, "BIND")]
public sealed class BindCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        var search = new SearchContext();
        var target = Engine.Parser.ReadWord(context.Index, ref instruction, buffer);
        if (Engine.Parser.TryEvalTarget(context.Index, ref search, target))
        {
            var targetActor = Engine.Actors[search.Index];
            context.Actor.Pointer = targetActor.Pointer;
            context.Actor.Length = targetActor.Length;
            instruction = 0;
        }
    }
}