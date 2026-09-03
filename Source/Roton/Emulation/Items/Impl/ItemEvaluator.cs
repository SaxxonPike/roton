using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ItemEvaluator(
    IItemList items,
    IParser parser)
    : IItemEvaluator
{
    public bool TryEval(ref OopContext oopContext, ref Word instruction, out IItem? result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var name = parser.ReadWord(oopContext.Index, ref instruction, buffer);
        result = items.Get(name);
        return result != null;
    }
}