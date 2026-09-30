using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00004)]
internal struct OriginalCoord
{
    [FieldOffset(0x0000)] public Word X;
    [FieldOffset(0x0002)] public Word Y;
}