using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00033)]
internal struct OriginalString50
{
    [FieldOffset(0x0000)] public HWord Length;
    [FieldOffset(0x0001)] public HWord Value; // array[50]
}