using System;
using Roton.Emulation.Colors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TileEvaluator(
    IParser parser,
    IColorList colors,
    IElementList elements)
    : ITileEvaluator
{
    public bool TryEval(ref OopContext context, ref Word instruction, out Tile result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var word = parser.ReadWord(context.Index, ref instruction, buffer);
        var success = false;
        result = new Tile(0, 0);

        if (colors.Get(word) is { Value: > 0 } color)
        {
            result.Color = color.Value;
            word = parser.ReadWord(context.Index, ref instruction, buffer);
        }

        var elementId = elements.IndexOf(word);
        if (elementId >= 0)
        {
            success = true;
            result.Id = elementId;
        }

        return success;
    }

}