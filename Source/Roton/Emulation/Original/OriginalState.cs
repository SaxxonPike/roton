using System;
using System.Collections.Generic;
using System.Diagnostics;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalState : IState
{
    public OriginalState(
        IMemory memory, 
        IHeap heap, 
        IEngineResourceService engineResourceService)
    {
        Memory = memory;
        Heap = heap;
        EngineResourceService = engineResourceService;

        Memory.Write(0x0000, EngineResourceService.GetMemoryData());
        DefaultActor = new Actor(Memory, Heap, 0x0076);
        LineChars = new ByteString(Memory, 0x0098);
        ProgressAnimation = new ProgressAnimation(Memory, 0x00B2);
        ProgressColors = new Int8List(Memory, 0x00AA, 8);
        SoundBuffer = new SoundBufferList(memory, 0x7E90);
        StarChars = new ByteString(Memory, 0x0336);
        TransporterHChars = new ByteString(Memory, 0x0236);
        TransporterVChars = new ByteString(Memory, 0x0136);
        Vector4 = new Int16List(Memory, 0x0062, 8);
        Vector8 = new Int16List(Memory, 0x0042, 16);
    }

    private IMemory Memory { [DebuggerStepThrough] get; }

    private IHeap Heap { [DebuggerStepThrough] get; }

    private IEngineResourceService EngineResourceService { [DebuggerStepThrough] get; }

    public int MainTime
    {
        get => Memory.FastRead16(0x740A);
        set => Memory.FastWrite16(0x740A, value);
    }

    public int VisibleTileCount
    {
        get => Memory.FastRead16(0x4ACC);
        set => Memory.FastWrite16(0x4ACC, value);
    }

    public bool AboutShown
    {
        get => Memory.ReadBool(0x7A60);
        set => Memory.WriteBool(0x7A60, value);
    }

    public int ActIndex
    {
        get => Memory.FastRead16(0x7406);
        set => Memory.FastWrite16(0x7406, value);
    }

    public int ActorCount
    {
        get => Memory.FastRead16(0x31CD);
        set => Memory.FastWrite16(0x31CD, value);
    }

    public int BoardCount
    {
        get => Memory.FastRead16(0x45BE);
        set => Memory.FastWrite16(0x45BE, value);
    }

    public ref Tile BorderTile => ref Memory.GetRef<Tile>(0x0072);

    public bool BreakGameLoop
    {
        get => Memory.ReadBool(0x4AC6);
        set => Memory.WriteBool(0x4AC6, value);
    }

    public bool CancelScroll
    {
        get => Memory.ReadBool(0x7B66);
        set => Memory.WriteBool(0x7B66, value);
    }

    public IActor DefaultActor { get; }

    public string DefaultBoardName
    {
        get => Memory.ReadString(0x241E);
        set => Memory.WriteString(0x241E, value);
    }

    public string DefaultSaveName
    {
        get => Memory.ReadString(0x23EA);
        set => Memory.WriteString(0x23EA, value);
    }

    public string DefaultWorldName
    {
        get => Memory.ReadString(0x2452);
        set => Memory.WriteString(0x2452, value);
    }

    public ref Tile EdgeTile => ref Memory.GetRef<Tile>(0x0074);

    public bool EditorMode
    {
        get => Memory.ReadBool(0x740C);
        set => Memory.WriteBool(0x740C, value);
    }

    public int ForestIndex { get; set; }

    public int GameCycle
    {
        get => Memory.FastRead16(0x7404);
        set => Memory.FastWrite16(0x7404, value);
    }

    public bool GameOver
    {
        get => Memory.ReadBool(0x7C8D);
        set => Memory.WriteBool(0x7C8D, value);
    }

    public bool GamePaused
    {
        get => Memory.ReadBool(0x7408);
        set => Memory.WriteBool(0x7408, value);
    }

    public bool GameQuiet
    {
        get => Memory.ReadBool(0x7C8C);
        set => Memory.WriteBool(0x7C8C, value);
    }

    public int GameSpeed
    {
        get => Memory.Read8(0x4ACE);
        set => Memory.Write8(0x4ACE, value);
    }

    public int GameWaitTime
    {
        get => Memory.FastRead16(0x7402);
        set => Memory.FastWrite16(0x7402, value);
    }

    public bool Init
    {
        get => Memory.ReadBool(0x7B60);
        set => Memory.WriteBool(0x7B60, value);
    }

    public bool KeyArrow
    {
        get => Memory.ReadBool(0x7C7E);
        set => Memory.WriteBool(0x7C7E, value);
    }

    public EngineKeyCode KeyPressed
    {
        get => (EngineKeyCode) Memory.Read8(0x7C70);
        set => Memory.Write8(0x7C70, (int) value);
    }

    public bool KeyShift
    {
        get => Memory.ReadBool(0x7C6C);
        set => Memory.WriteBool(0x7C6C, value);
    }

    public ref Vector KeyVector => ref Memory.GetRef<Vector>(0x7C68);

    public IReadOnlyList<int> LineChars { get; }
    
    public IReadOnlyList<string> ProgressAnimation { get; }
    
    public IReadOnlyList<int> ProgressColors { get; }

    public string Message
    {
        get => Memory.ReadString(0x456E);
        set => Memory.WriteString(0x456E, value);
    }

    public string Message2 { get; set; }

    public int OopByte
    {
        get => Memory.Read8(0x740E);
        set => Memory.Write8(0x740E, value);
    }

    public int OopNumber
    {
        get => Memory.FastRead16(0x7426);
        set => Memory.FastWrite16(0x7426, value);
    }

    public string OopWord
    {
        get => Memory.ReadString(0x7410);
        set => Memory.WriteString(0x7410, value);
    }

    public int PlayerElement
    {
        get => Memory.FastRead16(0x4AC8);
        set => Memory.FastWrite16(0x4AC8, value);
    }

    public int PlayerTime
    {
        get => Memory.FastRead16(0x4920);
        set => Memory.FastWrite16(0x4920, value);
    }

    public bool QuitEngine
    {
        get => Memory.ReadBool(0x4AC5);
        set => Memory.WriteBool(0x4AC5, value);
    }

    public ISoundBufferList SoundBuffer { get; }

    public bool SoundPlaying
    {
        get => Memory.ReadBool(0x7F9A);
        set => Memory.WriteBool(0x7F9A, value);
    }

    public int SoundPriority
    {
        get => Memory.FastRead16(0x7C8E);
        set => Memory.FastWrite16(0x7C8E, value);
    }

    public int SoundTicks
    {
        get => Memory.Read8(0x7E8F);
        set => Memory.Write8(0x7E8F, value);
    }

    public IReadOnlyList<int> StarChars { get; }

    public int StartBoard
    {
        get => Memory.FastRead16(0x4ACA);
        set => Memory.FastWrite16(0x4ACA, value);
    }

    public IReadOnlyList<int> TransporterHChars { get; }

    public IReadOnlyList<int> TransporterVChars { get; }

    public IReadOnlyList<int> Vector4 { get; }

    public IReadOnlyList<int> Vector8 { get; }

    public IReadOnlyList<int> WebChars { get; } = new List<int>();

    public string WorldFileName
    {
        get => Memory.ReadString(0x23B6);
        set => Memory.WriteString(0x23B6, value);
    }

    public bool WorldLoaded
    {
        get => Memory.ReadBool(0x7428);
        set => Memory.WriteBool(0x7428, value);
    }

    public ReadOnlySpan<char> GetOopWord(Span<char> buffer)
    {
        var span = Memory.ReadStringSpan(0x7410);
        Cp437.BytesToChars(span, buffer);
        return buffer.Slice(0, span.Length);
    }

    public void SetOopWord(ReadOnlySpan<char> buffer) => 
        Memory.WriteString(0x7410, buffer);
}