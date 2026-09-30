using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x0003D)]
internal struct SuperSmallString
{
    [FieldOffset(0x0000)] public HWord Length;
    [FieldOffset(0x0001)] public HWord Value; // array[60]
}