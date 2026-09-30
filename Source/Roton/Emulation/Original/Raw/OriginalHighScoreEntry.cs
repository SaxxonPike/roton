using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00035)]
internal struct OriginalHighScoreEntry
{
    [FieldOffset(0x0000)] public OriginalString50 Name;
    [FieldOffset(0x0033)] public Word Score;
}