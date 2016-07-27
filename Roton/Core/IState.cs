using System.Collections.Generic;

namespace Roton.Core
{
    public interface IState
    {
        IAlerts Alerts { get; }
        ITile BorderTile { get; }
        IColorList Colors { get; }
        IActor DefaultActor { get; }
        ITile EdgeTile { get; }
        IXyPair KeyVector { get; }
        IList<int> LineChars { get; }
        IList<int> SoundBuffer { get; }
        IList<int> StarChars { get; }
        ITimer PlayerTimer { get; }
        IList<int> TransporterHChars { get; }
        IList<int> TransporterVChars { get; }
        IList<int> Vector4 { get; }
        IList<int> Vector8 { get; }
        IList<int> WebChars { get; }
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
        int KeyPressed { get; set; }
        bool KeyShift { get; set; }
        int MainTime { get; set; }
        string Message { get; set; }
        string Message2 { get; set; }
        int OopByte { get; set; }
        int OopNumber { get; set; }
        string OopWord { get; set; }
        int PlayerElement { get; set; }
        int PlayerTime { get; set; }
        bool QuitZzt { get; set; }
        int SoundBufferLength { get; set; }
        bool SoundPlaying { get; set; }
        int SoundPriority { get; set; }
        int SoundTicks { get; set; }
        int StartBoard { get; set; }
        int VisibleTileCount { get; set; }
        string WorldFileName { get; set; }
        bool WorldLoaded { get; set; }
    }
}