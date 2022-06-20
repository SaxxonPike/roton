using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data;

public interface IState
{
    ITile BorderTile { get; }
    IActor DefaultActor { get; }
    ITile EdgeTile { get; }
    IXyPair KeyVector { get; }
    IReadOnlyList<int> LineChars { get; }
    ISoundBufferList SoundBuffer { get; }
    IReadOnlyList<int> StarChars { get; }
    IReadOnlyList<int> TransporterHChars { get; }
    IReadOnlyList<int> TransporterVChars { get; }
    IReadOnlyList<int> Vector4 { get; }
    IReadOnlyList<int> Vector8 { get; }
    IReadOnlyList<int> WebChars { get; }
    bool AboutShown { get; set; }
    int ActIndex { get; set; }
    int ActorCount { get; set; }
    int BoardCount { get; set; }
    bool BreakGameLoop { get; set; }
    bool CancelScroll { get; set; }
    string DefaultBoardName { get; set; }
    string DefaultSaveName { get; set; }
    string DefaultWorldName { get; set; }
    bool EditorMode { get; set; }
    int ForestIndex { get; set; }
    int GameCycle { get; set; }
    bool GameOver { get; set; }
    bool GamePaused { get; set; }
    bool GameQuiet { get; set; }
    int GameSpeed { get; set; }
    int GameWaitTime { get; set; }
    bool Init { get; set; }
    bool KeyArrow { get; set; }
    EngineKeyCode KeyPressed { get; set; }
    bool KeyShift { get; set; }
    string Message { get; set; }
    string Message2 { get; set; }
    int OopByte { get; set; }
    int OopNumber { get; set; }
    string OopWord { get; set; }
    int PlayerElement { get; set; }
    int PlayerTime { get; set; }
    bool QuitEngine { get; set; }
    bool SoundPlaying { get; set; }
    int SoundPriority { get; set; }
    int SoundTicks { get; set; }
    int StartBoard { get; set; }
    string WorldFileName { get; set; }
    bool WorldLoaded { get; set; }
}