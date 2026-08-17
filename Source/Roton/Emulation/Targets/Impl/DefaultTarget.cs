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

    public bool Execute(int index, ISearchContext context, ReadOnlySpan<char> term)
    {
        Span<char> buffer = stackalloc char[256];

        while (context.SearchIndex < Actors.Count)
        {
            if (Actors[context.SearchIndex].Pointer != 0)
            {
                var instruction = new Executable();
                var firstByte = Parser.ReadByte(context.SearchIndex, instruction);
                if (firstByte == 0x40)
                {
                    var name = Parser.ReadWord(context.SearchIndex, instruction, buffer);
                    if (name.Equals(term, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            context.SearchIndex++;
        }
        return false;
    }
}