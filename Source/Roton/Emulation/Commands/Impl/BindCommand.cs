using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Targets;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BIND")]
[Context(Context.Super, "BIND")]
internal sealed class BindCommand(
    IActorList actors,
    IParser parser,
    ITargetEvaluator targetEvaluator,
    ICodeHeap heap)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        var search = new SearchContext();
        var target = parser.ReadWord(context.Index, ref instruction, buffer);

        if (targetEvaluator.TryEval(context.Index, ref search, target))
        {
            heap.Free(context.Actor.Pointer);
            var targetActor = actors[search.Index];
            context.Actor.Pointer = targetActor.Pointer;
            context.Actor.Length = targetActor.Length;
            instruction = 0;
        }
    }
}