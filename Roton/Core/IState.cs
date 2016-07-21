using System.Collections.Generic;

namespace Roton.Core
{
    public interface IState
    {
        bool AboutShown { get; set; }
        int ActIndex { get; set; }
        int ActorCount { get; set; }
        bool AlertAmmo { get; set; }
        bool AlertDark { get; set; }
        bool AlertEnergy { get; set; }
        bool AlertFake { get; set; }
        bool AlertForest { get; set; }
        bool AlertGem { get; set; }
        bool AlertNoAmmo { get; set; }
        bool AlertNoShoot { get; set; }
        bool AlertNotDark { get; set; }
        bool AlertNoTorch { get; set; }
        bool AlertTorch { get; set; }
        int BoardCount { get; set; }
        ITile BorderTile { get; }
        bool BreakGameLoop { get; set; }
        bool CancelScroll { get; set; }
        IColorList Colors { get; }
        IActor DefaultActor { get; }
        string DefaultBoardName { get; set; }
        string DefaultSaveName { get; set; }
        string DefaultWorldName { get; set; }
        ITile EdgeTile { get; }
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
        IXyPair KeyVector { get; }
        IList<int> LineChars { get; }
        int MainTime { get; set; }
        string Message { get; set; }
        string Message2 { get; set; }
        int OopByte { get; set; }
        int OopNumber { get; set; }
        string OopWord { get; set; }
        int PlayerElement { get; set; }
        int PlayerTime { get; set; }
        bool QuitZzt { get; set; }
        IList<int> SoundBuffer { get; }
        int SoundBufferLength { get; set; }
        bool SoundPlaying { get; set; }
        int SoundPriority { get; set; }
        int SoundTicks { get; set; }
        IList<int> StarChars { get; }
        int StartBoard { get; set; }
        IList<int> TransporterHChars { get; }
        IList<int> TransporterVChars { get; }
        IList<int> Vector4 { get; }
        IList<int> Vector8 { get; }
        int VisibleTileCount { get; set; }
        IList<int> WebChars { get; }
        string WorldFileName { get; set; }
        bool WorldLoaded { get; set; }
    }
}
