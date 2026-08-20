using System;
using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data;

public interface IState
{
    ref Tile BorderTile { get; }
    IActor DefaultActor { get; }
    ref Tile EdgeTile { get; }
    ref Vector KeyVector { get; }
    ref Vector KeyLastVector { get; }
    IReadOnlyList<int> LineChars { get; }
    IReadOnlyList<string> ProgressAnimation { get; }
    IReadOnlyList<int> ProgressColors { get; }
    ISoundBufferList SoundBuffer { get; }
    IReadOnlyList<int> StarChars { get; }
    IReadOnlyList<int> TransporterHChars { get; }
    IReadOnlyList<int> TransporterVChars { get; }
    IReadOnlyList<int> Vector4 { get; }
    IReadOnlyList<int> Vector8 { get; }
    IReadOnlyList<int> WebChars { get; }
    ref Bool AboutShown { get; }
    ref Word ActIndex { get; }
    ref Word ActorCount { get; }
    ref Word BoardCount { get; }
    ref Bool BreakGameLoop { get; }
    ref Bool CancelScroll { get; }
    string DefaultBoardName { get; set; }
    string DefaultSaveName { get; set; }
    string DefaultWorldName { get; set; }
    ref Bool EditorMode { get; }
    ref Word ForestIndex { get; }
    ref Word GameCycle { get; }
    ref Bool GameOver { get; }
    ref Bool GamePaused { get; }
    ref Bool GameQuiet { get; }
    ref HWord GameSpeed { get; }
    ref Word GameWaitTime { get; }
    ref Bool Init { get; }
    ref EngineKeyCode KeyPressed { get; }
    ref Bool KeyShift { get; }
    string Message { get; set; }
    string Message2 { get; set; }
    ref PChar OopByte { get; }
    ref Word OopNumber { get; }
    ref Word PlayerElement { get; }
    ref Bool QuitEngine { get; }
    ref Bool SoundPlaying { get; }
    ref Word SoundPriority { get; }
    ref HWord SoundTicks { get; }
    ref Word StartBoard { get; }
    string WorldFileName { get; set; }
    ref Bool WorldLoaded { get; }

    ReadOnlySpan<char> GetOopWord(Span<char> buffer);
    void SetOopWord(ReadOnlySpan<char> buffer);
}