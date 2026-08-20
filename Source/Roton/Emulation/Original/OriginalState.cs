using System;
using System.Collections.Generic;
using System.Diagnostics;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalState : IState
{
    private Word _forestIndex;

    public OriginalState(
        IMemory memory,
        ICodeHeap heap,
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

    private ICodeHeap Heap { [DebuggerStepThrough] get; }

    private IEngineResourceService EngineResourceService { [DebuggerStepThrough] get; }

    public ref Bool AboutShown => ref Memory.GetRef<Bool>(0x7A60);

    public ref Word ActIndex => ref Memory.GetRef<Word>(0x7406);

    public ref Word ActorCount => ref Memory.GetRef<Word>(0x31CD);

    public ref Word BoardCount => ref Memory.GetRef<Word>(0x45BE);

    public ref Tile BorderTile => ref Memory.GetRef<Tile>(0x0072);

    public ref Bool BreakGameLoop => ref Memory.GetRef<Bool>(0x4AC6);

    public ref Bool CancelScroll => ref Memory.GetRef<Bool>(0x7B66);

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

    public ref Bool EditorMode => ref Memory.GetRef<Bool>(0x740C);

    public ref Word ForestIndex => ref _forestIndex;

    public ref Word GameCycle => ref Memory.GetRef<Word>(0x7404);

    public ref Bool GameOver => ref Memory.GetRef<Bool>(0x7C8D);

    public ref Bool GamePaused => ref Memory.GetRef<Bool>(0x7408);

    public ref Bool GameQuiet => ref Memory.GetRef<Bool>(0x7C8C);

    public ref HWord GameSpeed => ref Memory.GetRef<HWord>(0x4ACE);

    public ref Word GameWaitTime => ref Memory.GetRef<Word>(0x7402);

    public ref Bool Init => ref Memory.GetRef<Bool>(0x7B60);

    public ref EngineKeyCode KeyPressed => ref Memory.GetRef<EngineKeyCode>(0x7C70);

    public ref Bool KeyShift => ref Memory.GetRef<Bool>(0x7C6C);

    public ref Vector KeyVector => ref Memory.GetRef<Vector>(0x7C68);

    public ref Vector KeyLastVector => ref Memory.GetRef<Vector>(0x7C84);

    public IReadOnlyList<int> LineChars { get; }

    public IReadOnlyList<string> ProgressAnimation { get; }

    public IReadOnlyList<int> ProgressColors { get; }

    public string Message
    {
        get => Memory.ReadString(0x456E);
        set => Memory.WriteString(0x456E, value);
    }

    public string Message2 { get; set; } = string.Empty;

    public ref PChar OopByte => ref Memory.GetRef<PChar>(0x740E);

    public ref Word OopNumber => ref Memory.GetRef<Word>(0x7426);

    public ref Word PlayerElement => ref Memory.GetRef<Word>(0x4AC8);

    public ref Bool QuitEngine => ref Memory.GetRef<Bool>(0x4AC5);

    public ISoundBufferList SoundBuffer { get; }

    public ref Bool SoundPlaying => ref Memory.GetRef<Bool>(0x7F9A);

    public ref Word SoundPriority => ref Memory.GetRef<Word>(0x7C8E);

    public ref HWord SoundTicks => ref Memory.GetRef<HWord>(0x7E8F);

    public IReadOnlyList<int> StarChars { get; }

    public ref Word StartBoard => ref Memory.GetRef<Word>(0x4ACA);

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

    public ref Bool WorldLoaded => ref Memory.GetRef<Bool>(0x7428);

    public ReadOnlySpan<char> GetOopWord(Span<char> buffer)
    {
        var span = Memory.ReadStringSpan(0x7410);
        Cp437.BytesToChars(span, buffer);
        return buffer.Slice(0, span.Length);
    }

    public void SetOopWord(ReadOnlySpan<char> buffer) =>
        Memory.WriteString(0x7410, buffer);
}