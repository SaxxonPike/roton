using System.Collections.Generic;
using Roton.Emulation.Execution;
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
        IGrammar Grammar { get; }
        IHud Hud { get; }
        IMemory Memory { get; }
        IActor Player { get; }
        IGameSerializer GameSerializer { get; }
        ISounds Sounds { get; }
        IState StateData { get; }
        string StoneText { get; }
        ITileGrid Tiles { get; }
        bool TitleScreen { get; }
        IWorld WorldData { get; }
        bool TorchesEnabled { get; }
        int ActorIndexAt(IXyPair location);
        int Adjacent(IXyPair location, int id);
        void Attack(int index, IXyPair location);
        bool BroadcastLabel(int sender, string label, bool force);
        void ClearSound();
        void ClearWorld();
        void Convey(IXyPair center, int direction);
        void Destroy(IXyPair location);
        AnsiChar Draw(IXyPair location);
        void EnterBoard();
        void ExecuteCode(int index, ICodeInstruction instructionSource, string name);
        void FadePurple();
        void FadeRed();
        void ForcePlayerColor(int index);
        IXyPair GetCardinalVector(int index);
        bool GetPlayerTimeElapsed(int interval);
        void Harm(int index);
        void MoveActor(int index, IXyPair location);
        void MoveActorOnRiver(int index);
        void PackBoard();
        byte[] EncodeMusic(string music);
        void PlaySound(int priority, byte[] sound);
        void Push(IXyPair location, IXyPair vector);
        void PushThroughTransporter(IXyPair location, IXyPair vector);
        void RaiseError(string error);
        int RandomNumber(int max);
        int RandomNumberDeterministic(int max);
        int ReadActorCodeNumber(int index, ICodeInstruction instructionSource);
        string ReadActorCodeWord(int index, ICodeInstruction instructionSource);
        int ReadKey();
        void RedrawBoard();
        void RemoveActor(int index);
        void RemoveItem(IXyPair location);
        IXyPair Rnd();
        IXyPair RndP(IXyPair vector);
        IXyPair Seek(IXyPair location);
        void SetBoard(int boardIndex);
        void SetMessage(int duration, IMessage message);
        void ShowInGameHelp();
        void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source);
        bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned);
        void Start();
        void Stop();
        void UnpackBoard(int boardIndex);
        void UpdateBoard(IXyPair location);
        void UpdateRadius(IXyPair location, RadiusMode mode);
        void UpdateStatus();
        void WaitForTick();
    }
}