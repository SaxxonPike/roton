using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class CoreBase : IDisplayInfo
    {
        public CoreBase()
        {
            this.Boards = new List<PackedBoard>();
            this.Memory = new Memory();
            this.Random = new Random();
            this.RandomDeterministic = new Random(0);
        }

        abstract public MemoryActorCollectionBase Actors { get; }

        public bool AboutShown
        {
            get { return StateData.AboutShown; }
            set { StateData.AboutShown = value; }
        }

        public int ActIndex
        {
            get { return StateData.ActIndex; }
            set { StateData.ActIndex = value; }
        }

        public int ActorCount
        {
            get { return StateData.ActorCount; }
            set { StateData.ActorCount = value; }
        }

        public bool AlertAmmo
        {
            get { return StateData.AlertAmmo; }
            set { StateData.AlertAmmo = value; }
        }

        public bool AlertDark
        {
            get { return StateData.AlertDark; }
            set { StateData.AlertDark = value; }
        }

        public bool AlertEnergy
        {
            get { return StateData.AlertEnergy; }
            set { StateData.AlertEnergy = value; }
        }

        public bool AlertFake
        {
            get { return StateData.AlertFake; }
            set { StateData.AlertFake = value; }
        }

        public bool AlertForest
        {
            get { return StateData.AlertForest; }
            set { StateData.AlertForest = value; }
        }

        public bool AlertGem
        {
            get { return StateData.AlertGem; }
            set { StateData.AlertGem = value; }
        }

        public bool AlertNoAmmo
        {
            get { return StateData.AlertNoAmmo; }
            set { StateData.AlertNoAmmo = value; }
        }

        public bool AlertNoShoot
        {
            get { return StateData.AlertNoShoot; }
            set { StateData.AlertNoShoot = value; }
        }

        public bool AlertNotDark
        {
            get { return StateData.AlertNotDark; }
            set { StateData.AlertNotDark = value; }
        }

        public bool AlertNoTorch
        {
            get { return StateData.AlertNoTorch; }
            set { StateData.AlertNoTorch = value; }
        }

        public bool AlertTorch
        {
            get { return StateData.AlertTorch; }
            set { StateData.AlertTorch = value; }
        }

        public int Ammo
        {
            get { return WorldData.Ammo; }
            set { WorldData.Ammo = value; }
        }

        public int Board
        {
            get { return WorldData.Board; }
            set { WorldData.Board = value; }
        }

        public int BoardCount
        {
            get { return StateData.BoardCount; }
            set { StateData.BoardCount = value; }
        }

        abstract public MemoryBoardBase BoardData { get; }

        public string BoardName
        {
            get { return BoardData.Name; }
            set { BoardData.Name = value; }
        }

        public IList<PackedBoard> Boards
        {
            get;
            private set;
        }

        public Tile BorderTile
        {
            get { return StateData.BorderTile; }
        }

        public bool BreakGameLoop
        {
            get { return StateData.BreakGameLoop; }
            set { StateData.BreakGameLoop = value; }
        }

        public Location Camera
        {
            get { return BoardData.Camera; }
            set { BoardData.Camera = value; }
        }

        public bool CancelScroll
        {
            get { return StateData.CancelScroll; }
            set { StateData.CancelScroll = value; }
        }

        public IList<string> Colors
        {
            get { return StateData.Colors; }
        }

        public bool Dark
        {
            get { return BoardData.Dark; }
            set { BoardData.Dark = value; }
        }

        public Actor DefaultActor
        {
            get { return StateData.DefaultActor; }
        }

        public string DefaultBoardName
        {
            get { return StateData.DefaultBoardName; }
            set { StateData.DefaultBoardName = value; }
        }

        public string DefaultSaveName
        {
            get { return StateData.DefaultSaveName; }
            set { StateData.DefaultSaveName = value; }
        }

        public string DefaultWorldName
        {
            get { return StateData.DefaultWorldName; }
            set { StateData.DefaultWorldName = value; }
        }

        public IFileSystem Disk
        {
            get;
            set;
        }

        abstract public Display Display { get; }

        public Tile EdgeTile
        {
            get { return StateData.EdgeTile; }
        }

        public bool EditorMode
        {
            get { return StateData.EditorMode; }
            set { StateData.EditorMode = value; }
        }

        abstract public MemoryElementCollectionBase Elements { get; }

        IList<Element> IDisplayInfo.Elements
        {
            get
            {
                return this.Elements;
            }
        }

        public Location Enter
        {
            get { return BoardData.Enter; }
            set { BoardData.Enter = value; }
        }

        public int EnergyCycles
        {
            get { return WorldData.EnergyCycles; }
            set { WorldData.EnergyCycles = value; }
        }

        public int ExitEast
        {
            get { return BoardData.ExitEast; }
            set { BoardData.ExitEast = value; }
        }

        public int ExitNorth
        {
            get { return BoardData.ExitNorth; }
            set { BoardData.ExitNorth = value; }
        }

        public int ExitSouth
        {
            get { return BoardData.ExitSouth; }
            set { BoardData.ExitSouth = value; }
        }

        public int ExitWest
        {
            get { return BoardData.ExitWest; }
            set { BoardData.ExitWest = value; }
        }

        public MemoryFlagArrayBase Flags
        {
            get { return WorldData.FlagMemory; }
        }

        public int GameCycle
        {
            get { return StateData.GameCycle; }
            set { StateData.GameCycle = value; }
        }

        public bool GameOver
        {
            get { return StateData.GameOver; }
            set { StateData.GameOver = value; }
        }

        public bool GamePaused
        {
            get { return StateData.GamePaused; }
            set { StateData.GamePaused = value; }
        }

        public bool GameQuiet
        {
            get { return StateData.GameQuiet; }
            set { StateData.GameQuiet = value; }
        }

        public int GameSpeed
        {
            get { return StateData.GameSpeed; }
            set { StateData.GameSpeed = value; }
        }

        public int GameWaitTime
        {
            get { return StateData.GameWaitTime; }
            set { StateData.GameWaitTime = value; }
        }

        public int Gems
        {
            get { return WorldData.Gems; }
            set { WorldData.Gems = value; }
        }

        public int Health
        {
            get { return WorldData.Health; }
            set { WorldData.Health = value; }
        }

        public Heap Heap
        {
            get { return Memory.Heap; }
        }

        public bool Init
        {
            get { return StateData.Init; }
            set { StateData.Init = value; }
        }

        public bool KeyArrow
        {
            get { return StateData.KeyArrow; }
            set { StateData.KeyArrow = value; }
        }

        public IKeyboard Keyboard
        {
            get;
            set;
        }

        public int KeyPressed
        {
            get { return StateData.KeyPressed; }
            set { StateData.KeyPressed = value; }
        }

        public MemoryKeyArray Keys
        {
            get { return WorldData.KeyMemory; }
        }

        IList<bool> IDisplayInfo.Keys
        {
            // for some reason this can't be implicit
            get { return (IList<bool>)WorldData.KeyMemory; }
        }

        public bool KeyShift
        {
            get { return StateData.KeyShift; }
            set { StateData.KeyShift = value; }
        }

        public Vector KeyVector
        {
            get { return StateData.KeyVector; }
        }

        public IList<int> LineChars
        {
            get { return StateData.LineChars; }
        }

        public bool Locked
        {
            get { return WorldData.Locked; }
            set { WorldData.Locked = value; }
        }

        public int MainTime
        {
            get { return StateData.MainTime; }
            set { StateData.MainTime = value; }
        }

        public Memory Memory
        {
            get;
            private set;
        }

        public string Message
        {
            get { return StateData.Message; }
            set { StateData.Message = value; }
        }

        public string Message2
        {
            get { return StateData.Message2; }
            set { StateData.Message2 = value; }
        }

        public int OOPByte
        {
            get { return StateData.OOPByte; }
            set { StateData.OOPByte = value; }
        }

        public int OOPNumber
        {
            get { return StateData.OOPNumber; }
            set { StateData.OOPNumber = value; }
        }

        public string OOPWord
        {
            get { return StateData.OOPWord; }
            set { StateData.OOPWord = value; }
        }

        public int PlayerElement
        {
            get { return StateData.PlayerElement; }
            set { StateData.PlayerElement = value; }
        }

        public int PlayerTime
        {
            get { return StateData.PlayerTime; }
            set { StateData.PlayerTime = value; }
        }

        public bool Quiet
        {
            get { return StateData.GameQuiet; }
            set { StateData.GameQuiet = value; }
        }

        public bool QuitZZT
        {
            get { return StateData.QuitZZT; }
            set { StateData.QuitZZT = value; }
        }

        private Random Random
        {
            get;
            set;
        }

        private Random RandomDeterministic
        {
            get;
            set;
        }

        public int RandomNumber(int max)
        {
            return Random.Next(max);
        }

        public int RandomNumberDeterministic(int max)
        {
            return RandomDeterministic.Next(max);
        }

        public bool RestartOnZap
        {
            get { return BoardData.RestartOnZap; }
            set { BoardData.RestartOnZap = value; }
        }

        public int Score
        {
            get { return WorldData.Score; }
            set { WorldData.Score = value; }
        }

        abstract public SerializerBase Serializer { get; }

        public int Shots
        {
            get { return BoardData.Shots; }
            set { BoardData.Shots = value; }
        }

        public IList<int> SoundBuffer
        {
            get { return StateData.SoundBuffer; }
        }

        public int SoundBufferLength
        {
            get { return StateData.SoundBufferLength; }
            set { StateData.SoundBufferLength = value; }
        }

        public bool SoundPlaying
        {
            get { return StateData.SoundPlaying; }
            set { StateData.SoundPlaying = value; }
        }

        public int SoundPriority
        {
            get { return StateData.SoundPriority; }
            set { StateData.SoundPriority = value; }
        }

        abstract public SoundsBase Sounds { get; }

        public int SoundTicks
        {
            get { return StateData.SoundTicks; }
            set { StateData.SoundTicks = value; }
        }

        public ISpeaker Speaker
        {
            get;
            set;
        }

        public IList<int> StarChars
        {
            get { return StateData.StarChars; }
        }

        public int StartBoard
        {
            get { return StateData.StartBoard; }
            set { StateData.StartBoard = value; }
        }

        abstract public MemoryStateBase StateData { get; }

        public int Stones
        {
            get { return WorldData.Stones; }
            set { WorldData.Stones = value; }
        }

        abstract public bool StonesEnabled
        {
            get;
        }

        public ITerminal Terminal
        {
            get { return Display.Terminal; }
            set { Display.Terminal = value; }
        }

        abstract public MemoryTileCollectionBase Tiles { get; }

        public int TimeLimit
        {
            get { return BoardData.TimeLimit; }
            set { BoardData.TimeLimit = value; }
        }

        public int TimePassed
        {
            get { return WorldData.TimePassed; }
            set { WorldData.TimePassed = value; }
        }

        public bool TitleScreen
        {
            get { return PlayerElement != Elements.PlayerId; }
        }

        public int TorchCycles
        {
            get { return WorldData.TorchCycles; }
            set { WorldData.TorchCycles = value; }
        }

        public int Torches
        {
            get { return WorldData.Torches; }
            set { WorldData.Torches = value; }
        }

        abstract public bool TorchesEnabled
        {
            get;
        }

        public IList<int> TransporterHChars
        {
            get { return StateData.TransporterHChars; }
        }

        public IList<int> TransporterVChars
        {
            get { return StateData.TransporterVChars; }
        }

        public IList<int> Vector4
        {
            get { return StateData.Vector4; }
        }

        public IList<int> Vector8
        {
            get { return StateData.Vector8; }
        }

        public int VisibleTileCount
        {
            get { return StateData.VisibleTileCount; }
            set { StateData.VisibleTileCount = value; }
        }

        public IList<int> WebChars
        {
            get { return StateData.WebChars; }
        }

        abstract public MemoryWorldBase WorldData { get; }

        public string WorldFileName
        {
            get { return StateData.WorldFileName; }
            set { StateData.WorldFileName = value; }
        }

        public bool WorldLoaded
        {
            get { return StateData.WorldLoaded; }
            set { StateData.WorldLoaded = value; }
        }

        public string WorldName
        {
            get { return WorldData.Name; }
            set { WorldData.Name = value; }
        }
    }
}
