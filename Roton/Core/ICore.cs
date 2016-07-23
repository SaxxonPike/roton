using System.Collections.Generic;
using Roton.FileIo;

namespace Roton.Core
{
    public interface ICore
    {
        IActorList Actors { get; }
        IAlerts Alerts { get; }
        IBoard BoardData { get; }
        IList<IPackedBoard> Boards { get; }
        ITile BorderTile { get; }
        IXyPair Camera { get; }
        IColorList Colors { get; }
        IActor DefaultActor { get; }
        IFileSystem Disk { get; }
        ITile EdgeTile { get; }
        IElementList Elements { get; }
        IXyPair Enter { get; }
        IFlagList Flags { get; }
        IGrammar Grammar { get; }
        int Height { get; }
        IHud Hud { get; }
        IKeyboard Keyboard { get; }
        IKeyList Keys { get; }
        IXyPair KeyVector { get; }
        IList<int> LineChars { get; }
        IMemory Memory { get; }
        IActor Player { get; }
        ISerializer Serializer { get; }
        IList<int> SoundBuffer { get; }
        ISounds Sounds { get; }
        ISpeaker Speaker { get; }
        IList<int> StarChars { get; }
        IState StateData { get; }
        bool StonesEnabled { get; }
        string StoneText { get; }
        ITerminal Terminal { get; }
        ITileGrid Tiles { get; }
        bool TitleScreen { get; }
        bool TorchesEnabled { get; }
        IList<int> TransporterHChars { get; }
        IList<int> TransporterVChars { get; }
        IList<int> Vector4 { get; }
        IList<int> Vector8 { get; }
        IList<int> WebChars { get; }
        int Width { get; }
        IWorld WorldData { get; }
        bool AboutShown { get; set; }
        int ActIndex { get; set; }
        int ActorCount { get; set; }
        int Ammo { get; set; }
        int Board { get; set; }
        int BoardCount { get; set; }
        string BoardName { get; set; }
        bool BreakGameLoop { get; set; }
        bool CancelScroll { get; set; }
        bool Dark { get; set; }
        string DefaultBoardName { get; set; }
        string DefaultSaveName { get; set; }
        string DefaultWorldName { get; set; }
        bool EditorMode { get; set; }
        int EnergyCycles { get; set; }
        int ExitEast { get; set; }
        int ExitNorth { get; set; }
        int ExitSouth { get; set; }
        int ExitWest { get; set; }
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
        int KeyPressed { get; set; }
        bool KeyShift { get; set; }
        bool Locked { get; set; }
        int MainTime { get; set; }
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
        int Shots { get; set; }
        bool SoundPlaying { get; set; }
        int SoundPriority { get; set; }
        int SoundTicks { get; set; }
        int StartBoard { get; set; }
        int Stones { get; set; }
        int TimeLimit { get; set; }
        int TimePassed { get; set; }
        int TorchCycles { get; set; }
        int Torches { get; set; }
        int VisibleTileCount { get; set; }
        string WorldFileName { get; set; }
        bool WorldLoaded { get; set; }
        string WorldName { get; set; }
        void ActBear(int index);
        void ActBlinkWall(int index);
        void ActBomb(int index);
        void ActBullet(int index);
        void ActClockwise(int index);
        void ActCounter(int index);
        void ActDragonPup(int index);
        void ActDuplicator(int index);
        void ActHead(int index);
        void ActLion(int index);
        void ActMessenger(int index);
        void ActMonitor(int index);
        void ActObject(int index);
        IActor ActorAt(IXyPair location);
        int ActorIndexAt(IXyPair location);
        void ActPairer(int index);
        void ActPlayer(int index);
        void ActPusher(int index);
        void ActRoton(int index);
        void ActRuffian(int index);
        void ActScroll(int index);
        void ActSegment(int index);
        void ActShark(int index);
        void ActSlime(int index);
        void ActSpider(int index);
        void ActSpinningGun(int index);
        void ActStar(int index);
        void ActStone(int index);
        void ActTiger(int index);
        void ActTransporter(int index);
        void Attack(int index, IXyPair location);
        void ClearBoard();
        void ClearSound();
        void ClearWorld();
        void Convey(IXyPair center, int direction);
        void Destroy(IXyPair location);
        AnsiChar Draw(IXyPair location);
        AnsiChar DrawBlinkWall(IXyPair location);
        AnsiChar DrawBomb(IXyPair location);
        AnsiChar DrawClockwise(IXyPair location);
        AnsiChar DrawCounter(IXyPair location);
        AnsiChar DrawDragonPup(IXyPair location);
        AnsiChar DrawDuplicator(IXyPair location);
        AnsiChar DrawLine(IXyPair location);
        AnsiChar DrawObject(IXyPair location);
        AnsiChar DrawPusher(IXyPair location);
        AnsiChar DrawSpinningGun(IXyPair location);
        AnsiChar DrawStar(IXyPair location);
        AnsiChar DrawStone(IXyPair location);
        AnsiChar DrawTransporter(IXyPair location);
        AnsiChar DrawWeb(IXyPair location);
        IElement ElementAt(IXyPair location);
        IXyPair GetCardinalVector(int index);
        IXyPair GetConveyorVector(int index);
        void InteractAmmo(IXyPair location, int index, IXyPair vector);
        void InteractBoardEdge(IXyPair location, int index, IXyPair vector);
        void InteractBomb(IXyPair location, int index, IXyPair vector);
        void InteractDoor(IXyPair location, int index, IXyPair vector);
        void InteractEnemy(IXyPair location, int index, IXyPair vector);
        void InteractEnergizer(IXyPair location, int index, IXyPair vector);
        void InteractFake(IXyPair location, int index, IXyPair vector);
        void InteractForest(IXyPair location, int index, IXyPair vector);
        void InteractGem(IXyPair location, int index, IXyPair vector);
        void InteractInvisible(IXyPair location, int index, IXyPair vector);
        void InteractKey(IXyPair location, int index, IXyPair vector);
        void InteractObject(IXyPair location, int index, IXyPair vector);
        void InteractPassage(IXyPair location, int index, IXyPair vector);
        void InteractPushable(IXyPair location, int index, IXyPair vector);
        void InteractScroll(IXyPair location, int index, IXyPair vector);
        void InteractSlime(IXyPair location, int index, IXyPair vector);
        void InteractStone(IXyPair location, int index, IXyPair vector);
        void InteractTorch(IXyPair location, int index, IXyPair vector);
        void InteractTransporter(IXyPair location, int index, IXyPair vector);
        void InteractWater(IXyPair location, int index, IXyPair vector);
        void MoveActor(int index, IXyPair location);
        void PackBoard();
        void PlaySound(int priority, byte[] sound);
        void Push(IXyPair location, IXyPair vector);
        int RandomNumber(int max);
        int RandomNumberDeterministic(int max);
        int ReadActorCodeByte(int index, ICodeInstruction instructionSource);
        string ReadActorCodeLine(int index, ICodeInstruction instructionSource);
        int ReadActorCodeNumber(int index, ICodeInstruction instructionSource);
        string ReadActorCodeWord(int index, ICodeInstruction instructionSource);
        void ReadInput();
        int ReadKey();
        void RedrawBoard();
        void RemoveItem(IXyPair location);
        IXyPair Rnd();
        IXyPair Seek(IXyPair location);
        void SetBoard(int boardIndex);
        void SetMessage(int duration, IMessage message);
        void Start();
        void Stop();
        void UnpackBoard(int boardIndex);
        void UpdateBoard(IXyPair location);
        void WaitForTick();
    }
}