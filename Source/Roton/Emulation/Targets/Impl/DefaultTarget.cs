using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DefaultTarget(IActors actors, IParser parser) : ITarget
{
    private IActors Actors => actors;

    private IParser Parser => parser;

    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        while (context.Index < Actors.Count)
        {
            if (Actors[context.Index].Pointer != 0)
            {
                var instruction = new Word();
                var firstByte = Parser.ReadByte(context.Index, ref instruction);
                if (firstByte == 0x40)
                {
                    var name = Parser.ReadWord(context.Index, ref instruction, buffer);
                    if (name.Equals(term, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            context.Index++;
        }
        return false;
    }
}