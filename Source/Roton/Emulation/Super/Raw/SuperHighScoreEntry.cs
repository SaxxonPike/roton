using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x0003F)]
internal struct SuperHighScoreEntry
{
    [FieldOffset(0x0000)] public SuperSmallString Name;
    [FieldOffset(0x003D)] public Word Score;
}