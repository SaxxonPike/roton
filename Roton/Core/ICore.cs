using System.Collections.Generic;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;

namespace Roton.Core
{
    internal interface ICore
    {
        IActorList Actors { get; }
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
        int Ammo { get; set; }
        int Board { get; set; }
        int BoardCount { get; set; }
        IBoard BoardData { get; }
        string BoardName { get; set; }
        IList<PackedBoard> Boards { get; }
        ITile BorderTile { get; }
        bool BreakGameLoop { get; set; }
        IXyPair Camera { get; set; }
        bool CancelScroll { get; set; }
        IColorList Colors { get; }
        bool Dark { get; set; }
        IActor DefaultActor { get; }
        string DefaultBoardName { get; set; }
        string DefaultSaveName { get; set; }
        string DefaultWorldName { get; set; }
        IFileSystem Disk { get; set; }
        IHud Hud { get; }
        ITile EdgeTile { get; }
        bool EditorMode { get; set; }
        IElementList Elements { get; }
        IXyPair Enter { get; set; }
        int EnergyCycles { get; set; }
        int ExitEast { get; set; }
        int ExitNorth { get; set; }
        int ExitSouth { get; set; }
        int ExitWest { get; set; }
        IFlagList Flags { get; }
        int GameCycle { get; set; }
        bool GameOver { get; set; }
        bool GamePaused { get; set; }
        bool GameQuiet { get; set; }
        int GameSpeed { get; set; }
        int GameWaitTime { get; set; }
        int Gems { get; set; }
        int Health { get; set; }
        bool Init { get; set; }
        bool KeyArrow { get; set; }
        IKeyboard Keyboard { get; set; }
        int KeyPressed { get; set; }
        IKeyList Keys { get; }
        bool KeyShift { get; set; }
        IXyPair KeyVector { get; }
        IList<int> LineChars { get; }
        bool Locked { get; set; }
        int MainTime { get; set; }
        IMemory Memory { get; }
        string Message { get; set; }
        string Message2 { get; set; }
        int OopByte { get; set; }
        int OopNumber { get; set; }
        string OopWord { get; set; }
        int PlayerElement { get; set; }
        int PlayerTime { get; set; }
        bool Quiet { get; set; }
        bool QuitZzt { get; set; }
        bool RestartOnZap { get; set; }
        int Score { get; set; }
        ISerializer Serializer { get; }
        int Shots { get; set; }
        IList<int> SoundBuffer { get; }
        bool SoundPlaying { get; set; }
        int SoundPriority { get; set; }
        SoundsBase Sounds { get; }
        int SoundTicks { get; set; }
        ISpeaker Speaker { get; set; }
        IList<int> StarChars { get; }
        int StartBoard { get; set; }
        IState StateData { get; }
        int Stones { get; set; }
        bool StonesEnabled { get; }
        ITerminal Terminal { get; set; }
        ITileGrid Tiles { get; }
        int TimeLimit { get; set; }
        int TimePassed { get; set; }
        bool TitleScreen { get; }
        int TorchCycles { get; set; }
        int Torches { get; set; }
        bool TorchesEnabled { get; }
        IList<int> TransporterHChars { get; }
        IList<int> TransporterVChars { get; }
        IList<int> Vector4 { get; }
        IList<int> Vector8 { get; }
        int VisibleTileCount { get; set; }
        IList<int> WebChars { get; }
        IWorld WorldData { get; }
        string WorldFileName { get; set; }
        bool WorldLoaded { get; set; }
        string WorldName { get; set; }
        int Height { get; }
        IActor Player { get; }
        int Width { get; }
        string StoneText { get; }
        int RandomNumber(int max);
        int RandomNumberDeterministic(int max);
        void Act_Bear(int index);
        void Act_BlinkWall(int index);
        void Act_Bomb(int index);
        void Act_Bullet(int index);
        void Act_Clockwise(int index);
        void Act_Counter(int index);
        void Act_DragonPup(int index);
        void Act_Duplicator(int index);
        void Act_Head(int index);
        void Act_Lion(int index);
        void Act_Messenger(int index);
        void Act_Monitor(int index);
        void Act_Object(int index);
        void Act_Pairer(int index);
        void Act_Player(int index);
        void Act_Pusher(int index);
        void Act_Roton(int index);
        void Act_Ruffian(int index);
        void Act_Scroll(int index);
        void Act_Segment(int index);
        void Act_Shark(int index);
        void Act_Slime(int index);
        void Act_Spider(int index);
        void Act_SpinningGun(int index);
        void Act_Star(int index);
        void Act_Stone(int index);
        void Act_Tiger(int index);
        void Act_Transporter(int index);
        void Interact_Ammo(IXyPair location, int index, IXyPair vector);
        void Interact_BoardEdge(IXyPair location, int index, IXyPair vector);
        void Interact_Bomb(IXyPair location, int index, IXyPair vector);
        void Interact_Door(IXyPair location, int index, IXyPair vector);
        void Interact_Enemy(IXyPair location, int index, IXyPair vector);
        void Interact_Energizer(IXyPair location, int index, IXyPair vector);
        void Interact_Fake(IXyPair location, int index, IXyPair vector);
        void Interact_Forest(IXyPair location, int index, IXyPair vector);
        void Interact_Gem(IXyPair location, int index, IXyPair vector);
        void Interact_Invisible(IXyPair location, int index, IXyPair vector);
        void Interact_Key(IXyPair location, int index, IXyPair vector);
        void Interact_Object(IXyPair location, int index, IXyPair vector);
        void Interact_Passage(IXyPair location, int index, IXyPair vector);
        void Interact_Pushable(IXyPair location, int index, IXyPair vector);
        void Interact_Scroll(IXyPair location, int index, IXyPair vector);
        void Interact_Slime(IXyPair location, int index, IXyPair vector);
        void Interact_Stone(IXyPair location, int index, IXyPair vector);
        void Interact_Torch(IXyPair location, int index, IXyPair vector);
        void Interact_Transporter(IXyPair location, int index, IXyPair vector);
        void Interact_Water(IXyPair location, int index, IXyPair vector);
        IActor ActorAt(IXyPair location);
        int ActorIndexAt(IXyPair location);
        void Attack(int index, IXyPair location);
        void ClearBoard();
        void ClearSound();
        void ClearWorld();
        void Convey(IXyPair center, int direction);
        void Destroy(IXyPair location);
        void RemoveItem(IXyPair location);
        void ReadInput();
        int ReadKey();
        void WaitForTick();
        AnsiChar Draw(IXyPair location);
        AnsiChar Draw_BlinkWall(IXyPair location);
        AnsiChar Draw_Bomb(IXyPair location);
        AnsiChar Draw_Clockwise(IXyPair location);
        AnsiChar Draw_Counter(IXyPair location);
        AnsiChar Draw_DragonPup(IXyPair location);
        AnsiChar Draw_Duplicator(IXyPair location);
        AnsiChar Draw_Line(IXyPair location);
        AnsiChar Draw_Object(IXyPair location);
        AnsiChar Draw_Pusher(IXyPair location);
        AnsiChar Draw_SpinningGun(IXyPair location);
        AnsiChar Draw_Star(IXyPair location);
        AnsiChar Draw_Stone(IXyPair location);
        AnsiChar Draw_Transporter(IXyPair location);
        AnsiChar Draw_Web(IXyPair location);
        void UpdateBoard(IXyPair location);
        void UnpackBoard(int boardIndex);
        void Start();
        void Stop();
        void SetBoard(int boardIndex);
        void PackBoard();
    }
}