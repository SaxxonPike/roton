using System.Collections.Generic;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.FileIo;

namespace Roton.Core
{
    public interface IEngine
    {
        IActorList Actors { get; }
        IAlerts Alerts { get; }
        IBoard Board { get; }
        IList<IPackedBoard> Boards { get; }
        IFileSystem Disk { get; }
        IElementList Elements { get; }
        IGameSerializer GameSerializer { get; }
        IGrammar Grammar { get; }
        IHud Hud { get; }
        IMemory Memory { get; }
        IActor Player { get; }
        ISoundSet SoundSet { get; }
        IState State { get; }
        string StoneText { get; }
        ITileGrid Tiles { get; }
        bool TitleScreen { get; }
        IWorld World { get; }
        int ActorIndexAt(IXyPair location);
        int Adjacent(IXyPair location, int id);
        void Attack(int index, IXyPair location);
        bool BroadcastLabel(int sender, string label, bool force);
        void ClearSound();
        void ClearWorld();
        void Convey(IXyPair center, int direction);
        void Destroy(IXyPair location);
        AnsiChar Draw(IXyPair location);
        ISound EncodeMusic(string music);
        void EnterBoard();
        void ExecuteCode(int index, IExecutable instructionSource, string name);
        bool ExecuteLabel(int sender, ISearchContext context, string prefix);
        void FadePurple();
        bool FindTile(ITile kind, IXyPair location);
        void ForcePlayerColor(int index);
        IXyPair GetCardinalVector(int index);
        bool GetPlayerTimeElapsed(int interval);
        void HandlePlayerInput(IActor actor, int hotkey);
        void Harm(int index);
        void MoveActor(int index, IXyPair location);
        void MoveActorOnRiver(int index);
        void PackBoard();
        void PlaySound(int priority, ISound sound, int offset, int length);
        void PlotTile(IXyPair location, ITile tile);
        void Push(IXyPair location, IXyPair vector);
        void PushThroughTransporter(IXyPair location, IXyPair vector);
        void RaiseError(string error);
        int RandomNumber(int max);
        int ReadActorCodeByte(int index, IExecutable instructionSource);
        string ReadActorCodeLine(int index, IExecutable instructionSource);
        int ReadActorCodeNumber(int index, IExecutable instructionSource);
        string ReadActorCodeWord(int index, IExecutable instructionSource);
        int ReadKey();
        void RedrawBoard();
        void RemoveActor(int index);
        void RemoveItem(IXyPair location);
        IXyPair Rnd();
        IXyPair RndP(IXyPair vector);
        int SearchActorCode(int index, string term);
        IXyPair Seek(IXyPair location);
        void SetBoard(int boardIndex);
        void SetMessage(int duration, IMessage message);
        void ShowInGameHelp();
        void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source);
        bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned);
        void Start();
        void Stop();
        int SyncRandomNumber(int max);
        void UnpackBoard(int boardIndex);
        void UpdateBoard(IXyPair location);
        void UpdateRadius(IXyPair location, RadiusMode mode);
        void UpdateStatus();
        void WaitForTick();
    }
}