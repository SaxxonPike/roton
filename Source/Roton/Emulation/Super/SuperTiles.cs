using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperTiles(IMemory memory, IElementList elementList) : Tiles(memory, elementList, 0x2BEB, 96, 80);