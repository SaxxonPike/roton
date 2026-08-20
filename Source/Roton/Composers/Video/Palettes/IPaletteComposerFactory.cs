using System;

namespace Roton.Composers.Video.Palettes;

public interface IPaletteComposerFactory
{
    IPaletteComposer Get(ReadOnlyMemory<byte> data);
}