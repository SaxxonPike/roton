using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    abstract internal class State
    {
        virtual public bool AboutShown { get; set; }
        virtual public int ActIndex { get; set; }
        virtual public int ActorCount { get; set; }
        virtual public bool AlertAmmo { get; set; }
        virtual public bool AlertDark { get; set; }
        virtual public bool AlertEnergy { get; set; }
        virtual public bool AlertFake { get; set; }
        virtual public bool AlertForest { get; set; }
        virtual public bool AlertGem { get; set; }
        virtual public bool AlertNoAmmo { get; set; }
        virtual public bool AlertNoShoot { get; set; }
        virtual public bool AlertNotDark { get; set; }
        virtual public bool AlertNoTorch { get; set; }
        virtual public bool AlertTorch { get; set; }
        virtual public int BoardCount { get; set; }
        virtual public Tile BorderTile { get; protected set; }
        virtual public bool BreakGameLoop { get; set; }
        virtual public bool CancelScroll { get; set; }
        virtual public IList<string> Colors { get; protected set; }
        virtual public Actor DefaultActor { get; protected set; }
        virtual public string DefaultBoardName { get; set; }
        virtual public string DefaultSaveName { get; set; }
        virtual public string DefaultWorldName { get; set; }
        virtual public Tile EdgeTile { get; protected set; }
        virtual public bool EditorMode { get; set; }
        virtual public int ForestIndex { get; set; }
        virtual public int GameCycle { get; set; }
        virtual public bool GameOver { get; set; }
        virtual public bool GamePaused { get; set; }
        virtual public bool GameQuiet { get; set; }
        virtual public int GameSpeed { get; set; }
        virtual public int GameWaitTime { get; set; }
        virtual public bool Init { get; set; }
        virtual public bool KeyArrow { get; set; }
        virtual public int KeyPressed { get; set; }
        virtual public bool KeyShift { get; set; }
        virtual public Vector KeyVector { get; set; }
        virtual public IList<int> LineChars { get; protected set; }
        virtual public int MainTime { get; set; }
        virtual public string Message { get; set; }
        virtual public string Message2 { get; set; }
        virtual public int OOPByte { get; set; }
        virtual public int OOPNumber { get; set; }
        virtual public string OOPWord { get; set; }
        virtual public int PlayerElement { get; set; }
        virtual public int PlayerTime { get; set; }
        virtual public bool QuitZZT { get; set; }
        virtual public IList<int> SoundBuffer { get; protected set; }
        virtual public bool SoundPlaying { get; set; }
        virtual public int SoundPriority { get; set; }
        virtual public int SoundTicks { get; set; }
        virtual public IList<int> StarChars { get; protected set; }
        virtual public int StartBoard { get; set; }
        virtual public IList<int> TransporterHChars { get; protected set; }
        virtual public IList<int> TransporterVChars { get; protected set; }
        virtual public IList<int> Vector4 { get; protected set; }
        virtual public IList<int> Vector8 { get; protected set; }
        virtual public int VisibleTileCount { get; set; }
        virtual public IList<int> WebChars { get; protected set; }
        virtual public string WorldFileName { get; set; }
        virtual public bool WorldLoaded { get; set; }
    }
}
