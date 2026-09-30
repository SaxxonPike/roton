using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x0203A)]
internal struct OriginalTextWindowState
{
    [FieldOffset(0x0000)] public Bool Selectable;
    [FieldOffset(0x0002)] public Word LineCount;
    [FieldOffset(0x0004)] public Word LinePos;
    [FieldOffset(0x0006)] public DWord Lines; // array[1024]
    [FieldOffset(0x1006)] public OriginalString20 Hyperlink;
    [FieldOffset(0x101B)] public OriginalString50 Title;
    [FieldOffset(0x104E)] public OriginalString50 LoadedFilename;
    [FieldOffset(0x1081)] public OriginalString160 ScreenCopy; // array[25]
}