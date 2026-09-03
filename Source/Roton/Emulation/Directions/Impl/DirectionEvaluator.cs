using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DirectionEvaluator(
    IParser parser,
    IDirectionList directions)
    : IDirectionEvaluator
{
    public bool TryEval(ref OopContext oopContext, ref Word instruction, out Vector result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var name = parser.ReadWord(oopContext.Index, ref instruction, buffer);
        var direction = directions.Get(name);

        if (direction?.Execute(ref oopContext, ref instruction) is not { } temp)
        {
            result = default;
            return false;
        }

        result = temp;
        return true;
    }
}