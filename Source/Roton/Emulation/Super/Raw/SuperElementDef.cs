using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

[StructLayout(LayoutKind.Explicit, Size = 0x000C2)]
internal struct SuperElementDef
{
    [FieldOffset(0x0000)] public PChar Character;
    [FieldOffset(0x0001)] public HWord Color;
    [FieldOffset(0x0002)] public Bool Destructible;
    [FieldOffset(0x0003)] public Bool Pushable;
    [FieldOffset(0x0004)] public Bool PlaceableOnTop;
    [FieldOffset(0x0005)] public Bool Walkable;
    [FieldOffset(0x0006)] public Bool HasDrawProc;
    [FieldOffset(0x0007)] public DWord DrawProc; // (x, y: integer; var ch: byte)
    [FieldOffset(0x000B)] public Word Cycle;
    [FieldOffset(0x000D)] public DWord TickProc; // (statId: integer)
    [FieldOffset(0x0011)] public DWord TouchProc; // (x, y: integer; sourceStatId: integer; var deltaX, deltaY: integer)
    [FieldOffset(0x0015)] public Word EditorCategory;
    [FieldOffset(0x0017)] public PChar EditorShortcut;
    [FieldOffset(0x0018)] public SuperString20 Name;
    [FieldOffset(0x002D)] public SuperString20 CategoryName;
    [FieldOffset(0x0042)] public SuperString20 Param1Name;
    [FieldOffset(0x0057)] public SuperString20 Param2Name;
    [FieldOffset(0x006C)] public SuperString20 ParamBulletTypeName;
    [FieldOffset(0x0081)] public SuperString20 ParamBoardName;
    [FieldOffset(0x0096)] public SuperString20 ParamDirName;
    [FieldOffset(0x00AB)] public SuperString20 ParamTextName;
    [FieldOffset(0x00C0)] public Word ScoreValue;
}