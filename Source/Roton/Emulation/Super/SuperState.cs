using System;
using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperState : IState
{
    private Bool _aboutShown;
    private Tile _borderTile;
    private Bool _cancelScroll;
    private Bool _init;

    public SuperState(IMemory memory, IEngineResourceService engineResourceService, ICodeHeap heap)
    {
        _memory = memory;

        _memory.Write(0x0000, engineResourceService.GetMemoryData());
        DefaultActor = new Actor(_memory, heap, 0x2262);
        LineChars = new ByteString(_memory, 0x22BA);
        ProgressAnimation = new ProgressAnimation(_memory, 0x21C0);
        ProgressColors = new Int8List(_memory, 0x21B8, 8);
        SoundBuffer = new SoundBufferList(memory, 0xCF9E);
        StarChars = new ByteString(_memory, 0x2064);
        TransporterHChars = new ByteString(_memory, 0x1F64);
        TransporterVChars = new ByteString(_memory, 0x1E64);
        Vector4 = new Int16List(_memory, 0x2250, 8);
        Vector8 = new Int16List(_memory, 0x2230, 16);
        WebChars = new ByteString(_memory, 0x227C);
    }

    private readonly IMemory _memory;

    public ref Bool AboutShown => 
        ref _aboutShown;

    public ref Word ActIndex => 
        ref _memory.GetRef<Word>(0xB95A);

    public ref Word ActorCount => 
        ref _memory.GetRef<Word>(0x6AB3);

    public ref Word BoardCount => 
        ref _memory.GetRef<Word>(0x7784);

    public ref Tile BorderTile => 
        ref _borderTile;

    public ref Bool BreakGameLoop => 
        ref _memory.GetRef<Bool>(0x7C9E);

    public ref Bool CancelScroll => 
        ref _cancelScroll;

    public IActor DefaultActor { get; }

    public string DefaultBoardName
    {
        get => _memory.ReadString(0x2B32);
        set => _memory.WriteString(0x2B32, value);
    }

    public string DefaultSaveName
    {
        get => _memory.ReadString(0x2AF4);
        set => _memory.WriteString(0x2AF4, value);
    }

    public string DefaultWorldName
    {
        get => _memory.ReadString(0x2B70);
        set => _memory.WriteString(0x2B70, value);
    }

    public ref Tile EdgeTile => 
        ref _memory.GetRef<Tile>(0x2260);

    public ref Bool EditorMode => 
        ref _memory.GetRef<Bool>(0xB960);

    public ref Word ForestIndex => 
        ref _memory.GetRef<Word>(0x2334);

    public ref Word GameCycle => 
        ref _memory.GetRef<Word>(0xB958);

    public ref Bool GameOver => 
        ref _memory.GetRef<Bool>(0xCD9B);

    public ref Bool GamePaused => 
        ref _memory.GetRef<Bool>(0xB95C);

    public ref Bool GameQuiet => 
        ref _memory.GetRef<Bool>(0xCD9A);

    public ref HWord GameSpeed => 
        ref _memory.GetRef<HWord>(0x7CA4);

    public ref Word GameWaitTime => 
        ref _memory.GetRef<Word>(0xB956);

    public ref Bool Init => 
        ref _init;

    public ref EngineKeyCode KeyPressed => 
        ref _memory.GetRef<EngineKeyCode>(0xCC76);

    public ref Bool KeyShift => 
        ref _memory.GetRef<Bool>(0xCC72);

    public ref Vector KeyVector => 
        ref _memory.GetRef<Vector>(0xCC6E);

    public ref Vector KeyLastVector => 
        ref _memory.GetRef<Vector>(0xCC8A);

    public IRefList<PChar> LineChars { get; }

    public IReadOnlyList<string> ProgressAnimation { get; }

    public IRefList<HWord> ProgressColors { get; }

    public string Message
    {
        get => _memory.ReadString(0x7C22);
        set => _memory.WriteString(0x7C22, value);
    }

    public string Message2
    {
        get => _memory.ReadString(0x7C60);
        set => _memory.WriteString(0x7C60, value);
    }

    public ref PChar OopByte => 
        ref _memory.GetRef<PChar>(0xB962);

    public ref Word OopNumber => 
        ref _memory.GetRef<Word>(0xB97A);

    public ref Word PlayerElement => 
        ref _memory.GetRef<Word>(0x7CA0);

    public ref Bool QuitEngine => 
        ref _memory.GetRef<Bool>(0x7C9D);

    public ISoundBufferList SoundBuffer { get; }

    public ref Bool SoundPlaying => 
        ref _memory.GetRef<Bool>(0xD0A8);

    public ref Word SoundPriority => 
        ref _memory.GetRef<Word>(0xCD9C);

    public ref HWord SoundTicks => 
        ref _memory.GetRef<HWord>(0xCF9D);

    public IRefList<PChar> StarChars { get; }

    public ref Word StartBoard => 
        ref _memory.GetRef<Word>(0x7CA2);

    public IRefList<PChar> TransporterHChars { get; }

    public IRefList<PChar> TransporterVChars { get; }

    public IRefList<Word> Vector4 { get; }

    public IRefList<Word> Vector8 { get; }

    public IRefList<PChar> WebChars { get; }

    public string WorldFileName
    {
        get => _memory.ReadString(0x2AB6);
        set => _memory.WriteString(0x2AB6, value);
    }

    public ref Bool WorldLoaded => 
        ref _memory.GetRef<Bool>(0xB97C);

    public ReadOnlySpan<char> GetOopWord(Span<char> buffer)
    {
        var span = _memory.ReadStringSpan(0xB964);
        Cp437.BytesToChars(span, buffer);
        return buffer.Slice(0, span.Length);
    }

    public void SetOopWord(ReadOnlySpan<char> buffer) =>
        _memory.WriteString(0xB964, buffer);
}