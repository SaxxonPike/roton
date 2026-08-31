using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

/// <summary>
/// If the target term is not known, this is the default target resolution behavior.
/// </summary>
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DefaultTarget(IActorList actors, IParser parser) : ITarget
{
    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        while (context.Index < actors.Count)
        {
            if (actors[context.Index].Pointer != 0)
            {
                var instruction = new Word();
                var firstByte = parser.ReadByte(context.Index, ref instruction);

                if (firstByte == 0x40)
                {
                    var name = parser.ReadWord(context.Index, ref instruction, buffer);
                    if (name.Equals(term, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            context.Index++;
        }

        return false;
    }
}