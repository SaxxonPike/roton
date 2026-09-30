using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x004ED)]
internal struct OriginalWorld
{
    [FieldOffset(0x0000)] public Word BoardCount;
    [FieldOffset(0x0002)] public DWord BoardData; // array[101]
    [FieldOffset(0x0196)] public Word BoardLen; // array[101]
    [FieldOffset(0x0260)] public OriginalWorldInfo Info;
    [FieldOffset(0x0373)] public OriginalEditorStatSetting EditorStatSettings; // array[54]
}