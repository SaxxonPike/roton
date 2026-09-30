using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x000C3)]
internal struct OriginalElementDef
{
    [FieldOffset(0x0000)] public PChar Character;
    [FieldOffset(0x0001)] public HWord Color;
    [FieldOffset(0x0002)] public Bool Destructible;
    [FieldOffset(0x0003)] public Bool Pushable;
    [FieldOffset(0x0004)] public Bool VisibleInDark;
    [FieldOffset(0x0005)] public Bool PlaceableOnTop;
    [FieldOffset(0x0006)] public Bool Walkable;
    [FieldOffset(0x0007)] public Bool HasDrawProc;
    [FieldOffset(0x0008)] public DWord DrawProc; // (x, y: integer; var ch: byte)
    [FieldOffset(0x000C)] public Word Cycle;
    [FieldOffset(0x000E)] public DWord TickProc; // (statId: integer)
    [FieldOffset(0x0012)] public DWord TouchProc; // (x, y: integer; sourceStatId: integer; var deltaX, deltaY: integer)
    [FieldOffset(0x0016)] public Word EditorCategory;
    [FieldOffset(0x0018)] public PChar EditorShortcut;
    [FieldOffset(0x0019)] public OriginalString20 Name;
    [FieldOffset(0x002E)] public OriginalString20 CategoryName;
    [FieldOffset(0x0043)] public OriginalString20 Param1Name;
    [FieldOffset(0x0058)] public OriginalString20 Param2Name;
    [FieldOffset(0x006D)] public OriginalString20 ParamBulletTypeName;
    [FieldOffset(0x0082)] public OriginalString20 ParamBoardName;
    [FieldOffset(0x0097)] public OriginalString20 ParamDirName;
    [FieldOffset(0x00AC)] public OriginalString20 ParamTextName;
    [FieldOffset(0x00C1)] public Word ScoreValue;
}