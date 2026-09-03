using System;
using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalState : IState
{
    private Word _forestIndex;

    public OriginalState(
        IMemory memory,
        ICodeHeap heap,
        IEngineResourceService engineResourceService)
    {
        _memory = memory;

        _memory.Write(0x0000, engineResourceService.GetMemoryData());
        DefaultActor = new Actor(_memory, heap, 0x0076);
        LineChars = new ByteString(_memory, 0x0098);
        ProgressAnimation = new ProgressAnimation(_memory, 0x00B2);
        ProgressColors = new Int8List(_memory, 0x00AA, 8);
        SoundBuffer = new SoundBufferList(memory, 0x7E90);
        StarChars = new ByteString(_memory, 0x0336);
        TransporterHChars = new ByteString(_memory, 0x0236);
        TransporterVChars = new ByteString(_memory, 0x0136);
        Vector4 = new Int16List(_memory, 0x0062, 8);
        Vector8 = new Int16List(_memory, 0x0042, 16);
    }

    private readonly IMemory _memory;

    public ref Bool AboutShown =>
        ref _memory.GetRef<Bool>(0x7A60);

    public ref Word ActIndex =>
        ref _memory.GetRef<Word>(0x7406);

    public ref Word ActorCount =>
        ref _memory.GetRef<Word>(0x31CD);

    public ref Word BoardCount =>
        ref _memory.GetRef<Word>(0x45BE);

    public ref Tile BorderTile =>
        ref _memory.GetRef<Tile>(0x0072);

    public ref Bool BreakGameLoop =>
        ref _memory.GetRef<Bool>(0x4AC6);

    public ref Bool CancelScroll =>
        ref _memory.GetRef<Bool>(0x7B66);

    public IActor DefaultActor { get; }

    public string DefaultBoardName
    {
        get => _memory.ReadString(0x241E);
        set => _memory.WriteString(0x241E, value);
    }

    public string DefaultSaveName
    {
        get => _memory.ReadString(0x23EA);
        set => _memory.WriteString(0x23EA, value);
    }

    public string DefaultWorldName
    {
        get => _memory.ReadString(0x2452);
        set => _memory.WriteString(0x2452, value);
    }

    public ref Tile EdgeTile =>
        ref _memory.GetRef<Tile>(0x0074);

    public ref Bool EditorMode =>
        ref _memory.GetRef<Bool>(0x740C);

    public ref Word ForestIndex =>
        ref _forestIndex;

    public ref Word GameCycle =>
        ref _memory.GetRef<Word>(0x7404);

    public ref Bool GameOver =>
        ref _memory.GetRef<Bool>(0x7C8D);

    public ref Bool GamePaused =>
        ref _memory.GetRef<Bool>(0x7408);

    public ref Bool GameQuiet =>
        ref _memory.GetRef<Bool>(0x7C8C);

    public ref HWord GameSpeed =>
        ref _memory.GetRef<HWord>(0x4ACE);

    public ref Word GameWaitTime =>
        ref _memory.GetRef<Word>(0x7402);

    public ref Bool Init =>
        ref _memory.GetRef<Bool>(0x7B60);

    public ref EngineKeyCode KeyPressed =>
        ref _memory.GetRef<EngineKeyCode>(0x7C70);

    public ref Bool KeyShift =>
        ref _memory.GetRef<Bool>(0x7C6C);

    public ref Vector KeyVector =>
        ref _memory.GetRef<Vector>(0x7C68);

    public ref Vector KeyLastVector =>
        ref _memory.GetRef<Vector>(0x7C84);

    public IRefList<PChar> LineChars { get; }

    public IReadOnlyList<string> ProgressAnimation { get; }

    public IRefList<HWord> ProgressColors { get; }

    public string Message
    {
        get => _memory.ReadString(0x456E);
        set => _memory.WriteString(0x456E, value);
    }

    public string Message2 { get; set; } = string.Empty;

    public ref PChar OopByte =>
        ref _memory.GetRef<PChar>(0x740E);

    public ref Word OopNumber =>
        ref _memory.GetRef<Word>(0x7426);

    public ref Word PlayerElement =>
        ref _memory.GetRef<Word>(0x4AC8);

    public ref Bool QuitEngine =>
        ref _memory.GetRef<Bool>(0x4AC5);

    public ISoundBufferList SoundBuffer { get; }

    public ref Bool SoundPlaying =>
        ref _memory.GetRef<Bool>(0x7F9A);

    public ref Word SoundPriority =>
        ref _memory.GetRef<Word>(0x7C8E);

    public ref HWord SoundTicks =>
        ref _memory.GetRef<HWord>(0x7E8F);

    public IRefList<PChar> StarChars { get; }

    public ref Word StartBoard =>
        ref _memory.GetRef<Word>(0x4ACA);

    public IRefList<PChar> TransporterHChars { get; }

    public IRefList<PChar> TransporterVChars { get; }

    public IRefList<Word> Vector4 { get; }

    public IRefList<Word> Vector8 { get; }

    public IRefList<PChar> WebChars =>
        null!;

    public string WorldFileName
    {
        get => _memory.ReadString(0x23B6);
        set => _memory.WriteString(0x23B6, value);
    }

    public ref Bool WorldLoaded =>
        ref _memory.GetRef<Bool>(0x7428);

    public ReadOnlySpan<char> GetOopWord(Span<char> buffer)
    {
        var span = _memory.ReadStringSpan(0x7410);
        Cp437.BytesToChars(span, buffer);
        return buffer.Slice(0, span.Length);
    }

    public void SetOopWord(ReadOnlySpan<char> buffer) =>
        _memory.WriteString(0x7410, buffer);

    public Vector GetCardinalVector(int index) =>
        new(Vector4[index], Vector4[index + 4]);
}