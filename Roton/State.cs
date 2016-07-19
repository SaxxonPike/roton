using System.Collections.Generic;

namespace Roton
{
    internal abstract class State
    {
        public virtual bool AboutShown { get; set; }
        public virtual int ActIndex { get; set; }
        public virtual int ActorCount { get; set; }
        public virtual bool AlertAmmo { get; set; }
        public virtual bool AlertDark { get; set; }
        public virtual bool AlertEnergy { get; set; }
        public virtual bool AlertFake { get; set; }
        public virtual bool AlertForest { get; set; }
        public virtual bool AlertGem { get; set; }
        public virtual bool AlertNoAmmo { get; set; }
        public virtual bool AlertNoShoot { get; set; }
        public virtual bool AlertNotDark { get; set; }
        public virtual bool AlertNoTorch { get; set; }
        public virtual bool AlertTorch { get; set; }
        public virtual int BoardCount { get; set; }
        public virtual Tile BorderTile { get; protected set; }
        public virtual bool BreakGameLoop { get; set; }
        public virtual bool CancelScroll { get; set; }
        public virtual IList<string> Colors { get; protected set; }
        public virtual IActor DefaultActor { get; protected set; }
        public virtual string DefaultBoardName { get; set; }
        public virtual string DefaultSaveName { get; set; }
        public virtual string DefaultWorldName { get; set; }
        public virtual Tile EdgeTile { get; protected set; }
        public virtual bool EditorMode { get; set; }
        public virtual int ForestIndex { get; set; }
        public virtual int GameCycle { get; set; }
        public virtual bool GameOver { get; set; }
        public virtual bool GamePaused { get; set; }
        public virtual bool GameQuiet { get; set; }
        public virtual int GameSpeed { get; set; }
        public virtual int GameWaitTime { get; set; }
        public virtual bool Init { get; set; }
        public virtual bool KeyArrow { get; set; }
        public virtual int KeyPressed { get; set; }
        public virtual bool KeyShift { get; set; }
        public virtual Vector KeyVector { get; set; }
        public virtual IList<int> LineChars { get; protected set; }
        public virtual int MainTime { get; set; }
        public virtual string Message { get; set; }
        public virtual string Message2 { get; set; }
        public virtual int OopByte { get; set; }
        public virtual int OopNumber { get; set; }
        public virtual string OopWord { get; set; }
        public virtual int PlayerElement { get; set; }
        public virtual int PlayerTime { get; set; }
        public virtual bool QuitZzt { get; set; }
        public virtual IList<int> SoundBuffer { get; protected set; }
        public virtual bool SoundPlaying { get; set; }
        public virtual int SoundPriority { get; set; }
        public virtual int SoundTicks { get; set; }
        public virtual IList<int> StarChars { get; protected set; }
        public virtual int StartBoard { get; set; }
        public virtual IList<int> TransporterHChars { get; protected set; }
        public virtual IList<int> TransporterVChars { get; protected set; }
        public virtual IList<int> Vector4 { get; protected set; }
        public virtual IList<int> Vector8 { get; protected set; }
        public virtual int VisibleTileCount { get; set; }
        public virtual IList<int> WebChars { get; protected set; }
        public virtual string WorldFileName { get; set; }
        public virtual bool WorldLoaded { get; set; }
    }
}