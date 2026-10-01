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
        IEngineResourceService engineResourceService,
        ITimerFactory timerFactory,
        IFactory factory,
        IFacts facts)
    {
        _memory = memory;

        _memory.Write(0x0000, engineResourceService.GetMemoryData());
        DefaultActor = factory.Create<Actor>(facts.DefaultActorOffset, facts.ActorDataLength);
        LineChars = _memory.GetByteString(0x0424);
        PlayerTimer = timerFactory.Create(0x740E);
        ProgressAnimation = new ProgressAnimation(_memory, 0x035E);
        ProgressColors = new Int8List(_memory, 0x0356, 8);
        SoundBuffer = new SoundBufferList(memory, 0x8BF6);
        StarChars = _memory.GetByteString(0x0202);
        TransporterHChars = _memory.GetByteString(0x0102);
        TransporterVChars = _memory.GetByteString(0x0002);
        Vector4 = new Int16List(_memory, 0x03EE, 8);
        Vector8 = new Int16List(_memory, 0x03CE, 16);
    }

    private readonly IMemory _memory;

    public ref Bool AboutShown =>
        ref _memory.GetRef<Bool>(0x7A2C);

    public ref Word ActIndex =>
        ref _memory.GetRef<Word>(0x740A);

    public ref Word ActorCount =>
        ref _memory.GetRef<Word>(0x31D1);

    public ref Word BoardCount =>
        ref _memory.GetRef<Word>(0x45C2);

    public ref Tile BorderTile =>
        ref _memory.GetRef<Tile>(0x03FE);

    public ref Bool BreakGameLoop =>
        ref _memory.GetRef<Bool>(0x4ACA);

    public ref Bool CancelScroll =>
        ref _memory.GetRef<Bool>(0x8263);

    public IActor DefaultActor { get; }

    public string DefaultBoardName
    {
        get => _memory.ReadString(0x2422);
        set => _memory.WriteString(0x2422, value);
    }

    public string DefaultSaveName
    {
        get => _memory.ReadString(0x23EE);
        set => _memory.WriteString(0x23EE, value);
    }

    public string DefaultWorldName
    {
        get => _memory.ReadString(0x2456);
        set => _memory.WriteString(0x2456, value);
    }

    public ref Tile EdgeTile =>
        ref _memory.GetRef<Tile>(0x0400);

    public ref Bool EditorMode =>
        ref _memory.GetRef<Bool>(0x7410);

    public ref Word ForestIndex =>
        ref _forestIndex;

    public ref Word GameCycle =>
        ref _memory.GetRef<Word>(0x7408);

    public ref Bool GameOver =>
        ref _memory.GetRef<Bool>(0x89F3);

    public ref Bool GamePaused =>
        ref _memory.GetRef<Bool>(0x740C);

    public ref Bool GameQuiet =>
        ref _memory.GetRef<Bool>(0x89F2);

    public ref HWord GameSpeed =>
        ref _memory.GetRef<HWord>(0x4AD2);

    public ref Word GameWaitTime =>
        ref _memory.GetRef<Word>(0x7406);

    public ref Bool Init =>
        ref _memory.GetRef<Bool>(0x7BCD);

    public ref EngineKeyCode KeyPressed =>
        ref _memory.GetRef<EngineKeyCode>(0x88CE);

    public ref Bool KeyShift =>
        ref _memory.GetRef<Bool>(0x88CA);

    public ref Vector KeyVector =>
        ref _memory.GetRef<Vector>(0x88C6);

    public ref Vector KeyLastVector =>
        ref _memory.GetRef<Vector>(0x88E2);

    public IRefList<PChar> LineChars { get; }

    public IReadOnlyList<string> ProgressAnimation { get; }

    public IRefList<HWord> ProgressColors { get; }

    public string Message
    {
        get => _memory.ReadString(0x4572);
        set => _memory.WriteString(0x4572, value);
    }

    public string Message2 { get; set; } = string.Empty;

    public ref PChar OopByte =>
        ref _memory.GetRef<PChar>(0x7412);

    public ref Word OopNumber =>
        ref _memory.GetRef<Word>(0x742A);

    public ref Word PlayerElement =>
        ref _memory.GetRef<Word>(0x4ACC);

    public ref Bool QuitEngine =>
        ref _memory.GetRef<Bool>(0x4AC9);

    public ISoundBufferList SoundBuffer { get; }

    public ref Bool SoundPlaying =>
        ref _memory.GetRef<Bool>(0x8D00);

    public ref Word SoundPriority =>
        ref _memory.GetRef<Word>(0x89F4);

    public ref HWord SoundTicks =>
        ref _memory.GetRef<HWord>(0x8BF5);

    public IRefList<PChar> StarChars { get; }

    public ref Word StartBoard =>
        ref _memory.GetRef<Word>(0x4ACE);

    public IRefList<PChar> TransporterHChars { get; }

    public IRefList<PChar> TransporterVChars { get; }

    public IRefList<Word> Vector4 { get; }

    public IRefList<Word> Vector8 { get; }

    public IRefList<PChar> WebChars =>
        null!;

    public string WorldFileName
    {
        get => _memory.ReadString(0x23BA);
        set => _memory.WriteString(0x23BA, value);
    }

    public ref Bool WorldLoaded =>
        ref _memory.GetRef<Bool>(0x742D); // todo: it's fake, refactor this

    public ref Word SoundTimeCheckHSec =>
        ref _memory.GetRef<Word>(0x8D08);

    public ref Word TimerTicks =>
        ref _memory.GetRef<Word>(0x8D06);

    public ref Word SoundTimeCheckCounter =>
        ref _memory.GetRef<Word>(0x8D02);

    public ITimer PlayerTimer { get; }

    public ReadOnlySpan<char> GetOopWord(Span<char> buffer)
    {
        var span = _memory.ReadStringSpan(0x7414);
        Cp437.BytesToChars(span, buffer);
        return buffer.Slice(0, span.Length);
    }

    public void SetOopWord(ReadOnlySpan<char> buffer) =>
        _memory.WriteString(0x7414, buffer);
}