using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x00487)]
internal struct SuperWorld
{
    [FieldOffset(0x0000)] public Word BoardCount;
    [FieldOffset(0x0002)] public DWord BoardData; // array[33]
    [FieldOffset(0x0086)] public Word BoardLen; // array[33]
    [FieldOffset(0x00C8)] public SuperWorldInfo Info;
    [FieldOffset(0x0257)] public SuperEditorStatSetting EditorStatSettings; // array[80]
}