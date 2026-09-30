using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x01978)]
internal struct SuperTextWindowState
{
    [FieldOffset(0x0000)] public Bool Selectable;
    [FieldOffset(0x0002)] public Word LineCount;
    [FieldOffset(0x0004)] public Word LinePos;
    [FieldOffset(0x0006)] public DWord Lines; // array[1024]
    [FieldOffset(0x1006)] public SuperString20 Hyperlink;
    [FieldOffset(0x101B)] public SuperSmallString Title;
    [FieldOffset(0x1058)] public SuperSmallString LoadedFilename;
    [FieldOffset(0x1095)] public SuperString90 ScreenCopy; // array[25]
}