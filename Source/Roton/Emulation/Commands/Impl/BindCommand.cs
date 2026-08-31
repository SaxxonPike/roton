using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BIND")]
[Context(Context.Super, "BIND")]
internal sealed class BindCommand(
    IActorList actors,
    IParser parser)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        var search = new SearchContext();
        var target = parser.ReadWord(context.Index, ref instruction, buffer);
        if (parser.TryEvalTarget(context.Index, ref search, target))
        {
            var targetActor = actors[search.Index];
            context.Actor.Pointer = targetActor.Pointer;
            context.Actor.Length = targetActor.Length;
            instruction = 0;
        }
    }
}