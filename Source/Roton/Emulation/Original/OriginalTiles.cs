using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalTiles(IMemory memory, IElementList elements)
    : Tiles(memory, elements, 0x24B9, 60, 25);