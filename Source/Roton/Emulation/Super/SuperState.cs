using System;
using System.Collections.Generic;
using System.Diagnostics;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperState : IState
{
    private Bool _aboutShown;
    private Tile _borderTile;
    private Bool _cancelScroll;
    private Bool _init;

    public SuperState(IMemory memory, IEngineResourceService engineResourceService, ICodeHeap heap)
    {
        Memory = memory;
        EngineResourceService = engineResourceService;
        Heap = heap;

        Memory.Write(0x0000, EngineResourceService.GetMemoryData());
        DefaultActor = new Actor(Memory, Heap, 0x2262);
        LineChars = new ByteString(Memory, 0x22BA);
        ProgressAnimation = new ProgressAnimation(Memory, 0x21C0);
        ProgressColors = new Int8List(Memory, 0x21B8, 8);
        SoundBuffer = new SoundBufferList(memory, 0xCF9E);
        StarChars = new ByteString(Memory, 0x2064);
        TransporterHChars = new ByteString(Memory, 0x1F64);
        TransporterVChars = new ByteString(Memory, 0x1E64);
        Vector4 = new Int16List(Memory, 0x2250, 8);
        Vector8 = new Int16List(Memory, 0x2230, 16);
        WebChars = new ByteString(Memory, 0x227C);
    }

    private IMemory Memory { [DebuggerStepThrough] get; }

    private IEngineResourceService EngineResourceService { [DebuggerStepThrough] get; }

    private ICodeHeap Heap { [DebuggerStepThrough] get; }

    public ref Bool AboutShown => ref _aboutShown;

    public ref Word ActIndex => ref Memory.GetRef<Word>(0xB95A);

    public ref Word ActorCount => ref Memory.GetRef<Word>(0x6AB3);

    public ref Word BoardCount => ref Memory.GetRef<Word>(0x7784);

    public ref Tile BorderTile => ref _borderTile;

    public ref Bool BreakGameLoop => ref Memory.GetRef<Bool>(0x7C9E);

    public ref Bool CancelScroll => ref _cancelScroll;

    public IActor DefaultActor { get; }

    public string DefaultBoardName
    {
        get => Memory.ReadString(0x2B32);
        set => Memory.WriteString(0x2B32, value);
    }

    public string DefaultSaveName
    {
        get => Memory.ReadString(0x2AF4);
        set => Memory.WriteString(0x2AF4, value);
    }

    public string DefaultWorldName
    {
        get => Memory.ReadString(0x2B70);
        set => Memory.WriteString(0x2B70, value);
    }

    public ref Tile EdgeTile => ref Memory.GetRef<Tile>(0x2260);

    public ref Bool EditorMode => ref Memory.GetRef<Bool>(0xB960);

    public ref Word ForestIndex => ref Memory.GetRef<Word>(0x2334);

    public ref Word GameCycle => ref Memory.GetRef<Word>(0xB958);

    public ref Bool GameOver => ref Memory.GetRef<Bool>(0xCD9B);

    public ref Bool GamePaused => ref Memory.GetRef<Bool>(0xB95C);

    public ref Bool GameQuiet => ref Memory.GetRef<Bool>(0xCD9A);

    public ref HWord GameSpeed => ref Memory.GetRef<HWord>(0x7CA4);

    public ref Word GameWaitTime => ref Memory.GetRef<Word>(0xB956);

    public ref Bool Init => ref _init;

    public ref EngineKeyCode KeyPressed => ref Memory.GetRef<EngineKeyCode>(0xCC76);

    public ref Bool KeyShift => ref Memory.GetRef<Bool>(0xCC72);

    public ref Vector KeyVector => ref Memory.GetRef<Vector>(0xCC6E);

    public ref Vector KeyLastVector => ref Memory.GetRef<Vector>(0xCC8A);

    public IReadOnlyList<int> LineChars { get; }

    public IReadOnlyList<string> ProgressAnimation { get; }

    public IReadOnlyList<int> ProgressColors { get; }

    public string Message
    {
        get => Memory.ReadString(0x7C22);
        set => Memory.WriteString(0x7C22, value);
    }

    public string Message2
    {
        get => Memory.ReadString(0x7C60);
        set => Memory.WriteString(0x7C60, value);
    }

    public ref PChar OopByte => ref Memory.GetRef<PChar>(0xB962);

    public ref Word OopNumber => ref Memory.GetRef<Word>(0xB97A);

    public ref Word PlayerElement => ref Memory.GetRef<Word>(0x7CA0);

    public ref Bool QuitEngine => ref Memory.GetRef<Bool>(0x7C9D);

    public ISoundBufferList SoundBuffer { get; }

    public ref Bool SoundPlaying => ref Memory.GetRef<Bool>(0xD0A8);

    public ref Word SoundPriority => ref Memory.GetRef<Word>(0xCD9C);

    public ref HWord SoundTicks => ref Memory.GetRef<HWord>(0xCF9D);

    public IReadOnlyList<int> StarChars { get; }

    public ref Word StartBoard => ref Memory.GetRef<Word>(0x7CA2);

    public IReadOnlyList<int> TransporterHChars { get; }

    public IReadOnlyList<int> TransporterVChars { get; }

    public IReadOnlyList<int> Vector4 { get; }

    public IReadOnlyList<int> Vector8 { get; }

    public IReadOnlyList<int> WebChars { get; }

    public string WorldFileName
    {
        get => Memory.ReadString(0x2AB6);
        set => Memory.WriteString(0x2AB6, value);
    }

    public ref Bool WorldLoaded => ref Memory.GetRef<Bool>(0xB97C);

    public ReadOnlySpan<char> GetOopWord(Span<char> buffer)
    {
        var span = Memory.ReadStringSpan(0xB964);
        Cp437.BytesToChars(span, buffer);
        return buffer.Slice(0, span.Length);
    }

    public void SetOopWord(ReadOnlySpan<char> buffer) =>
        Memory.WriteString(0xB964, buffer);
}