using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00200)]
internal struct SuperDrumData
{
    [FieldOffset(0x0000)] public Word Len;
    [FieldOffset(0x0002)] public Word Data; // array[255]
}