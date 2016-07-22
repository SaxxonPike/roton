using System;
using System.Collections.Generic;
using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;
using Roton.FileIo;

namespace Roton.Emulation.Execution
{
    internal abstract partial class Core : IDisplayInfo, ICore
    {
        protected Core()
        {
            Boards = new List<IPackedBoard>();
            Memory = new Memory();
            Random = new Random();
            RandomDeterministic = new Random(0);
        }

        public abstract IActorList Actors { get; }

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

        public abstract IBoard BoardData { get; }

        public string BoardName
        {
            get { return BoardData.Name; }
            set { BoardData.Name = value; }
        }

        public IList<IPackedBoard> Boards { get; }

        public ITile BorderTile => StateData.BorderTile;

        public bool BreakGameLoop
        {
            get { return StateData.BreakGameLoop; }
            set { StateData.BreakGameLoop = value; }
        }

        public IXyPair Camera
        {
            get { return BoardData.Camera; }
            set { BoardData.Camera.CopyFrom(value); }
        }

        public bool CancelScroll
        {
            get { return StateData.CancelScroll; }
            set { StateData.CancelScroll = value; }
        }

        public IColorList Colors => StateData.Colors;

        public bool Dark
        {
            get { return BoardData.Dark; }
            set { BoardData.Dark = value; }
        }

        public IActor DefaultActor => StateData.DefaultActor;

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

        public IFileSystem Disk { get; set; }

        public abstract IHud Hud { get; }

        public ITile EdgeTile => StateData.EdgeTile;

        public bool EditorMode
        {
            get { return StateData.EditorMode; }
            set { StateData.EditorMode = value; }
        }

        public abstract IElementList Elements { get; }

        public IXyPair Enter
        {
            get { return BoardData.Enter; }
            set { BoardData.Enter.CopyFrom(value); }
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

        public IFlagList Flags => WorldData.Flags;

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

        public IKeyboard Keyboard { get; set; }

        public int KeyPressed
        {
            get { return StateData.KeyPressed; }
            set { StateData.KeyPressed = value; }
        }

        public IKeyList Keys => WorldData.Keys;

        public bool KeyShift
        {
            get { return StateData.KeyShift; }
            set { StateData.KeyShift = value; }
        }

        public IXyPair KeyVector => StateData.KeyVector;

        public IList<int> LineChars => StateData.LineChars;

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

        public IMemory Memory { get; }

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

        public int OopByte
        {
            get { return StateData.OopByte; }
            set { StateData.OopByte = value; }
        }

        public int OopNumber
        {
            get { return StateData.OopNumber; }
            set { StateData.OopNumber = value; }
        }

        public string OopWord
        {
            get { return StateData.OopWord; }
            set { StateData.OopWord = value; }
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

        public bool QuitZzt
        {
            get { return StateData.QuitZzt; }
            set { StateData.QuitZzt = value; }
        }

        private Random Random { get; }

        private Random RandomDeterministic { get; }

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

        public abstract ISerializer Serializer { get; }

        public int Shots
        {
            get { return BoardData.Shots; }
            set { BoardData.Shots = value; }
        }

        public IList<int> SoundBuffer => StateData.SoundBuffer;

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

        public abstract ISounds Sounds { get; }

        public int SoundTicks
        {
            get { return StateData.SoundTicks; }
            set { StateData.SoundTicks = value; }
        }

        public ISpeaker Speaker { get; set; }

        public IList<int> StarChars => StateData.StarChars;

        public int StartBoard
        {
            get { return StateData.StartBoard; }
            set { StateData.StartBoard = value; }
        }

        public abstract IState StateData { get; }

        public int Stones
        {
            get { return WorldData.Stones; }
            set { WorldData.Stones = value; }
        }

        public abstract bool StonesEnabled { get; }

        public ITerminal Terminal
        {
            get { return Hud.Terminal; }
            set { Hud.Terminal = value; }
        }

        public abstract ITileGrid Tiles { get; }

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

        public bool TitleScreen => PlayerElement != Elements.PlayerId;

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

        public abstract bool TorchesEnabled { get; }

        public IList<int> TransporterHChars => StateData.TransporterHChars;

        public IList<int> TransporterVChars => StateData.TransporterVChars;

        public IList<int> Vector4 => StateData.Vector4;

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(Vector4[index], Vector4[index + 4]);
        }

        public IList<int> Vector8 => StateData.Vector8;

        public IXyPair GetConveyorVector(int index)
        {
            return new Vector(Vector8[index], Vector8[index + 8]);
        }

        public int VisibleTileCount
        {
            get { return StateData.VisibleTileCount; }
            set { StateData.VisibleTileCount = value; }
        }

        public IList<int> WebChars => StateData.WebChars;

        public abstract IWorld WorldData { get; }

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