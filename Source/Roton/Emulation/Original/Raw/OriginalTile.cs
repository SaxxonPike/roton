using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00002)]
internal struct OriginalTile
{
    [FieldOffset(0x0000)] public HWord Id;
    [FieldOffset(0x0001)] public HWord Color;
}