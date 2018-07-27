using System;
using Roton.Emulation.Actions;
using Roton.Emulation.Cheats;
using Roton.Emulation.Commands;
using Roton.Emulation.Conditions;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Directions;
using Roton.Emulation.Draws;
using Roton.Emulation.Interactions;
using Roton.Emulation.Items;
using Roton.Emulation.Targets;

namespace Roton.Emulation.Core
{
    public interface IEngine
    {
        event EventHandler Exited;
        event EventHandler Tick;
        
        IActors Actors { get; }
        IAlerts Alerts { get; }
        IBoard Board { get; }
        ICheatList CheatList { get; }
        IColors Colors { get; }
        ICommandList CommandList { get; }
        IConditionList ConditionList { get; }
        IConfig Config { get; }
        IDirectionList DirectionList { get; }
        IElementList ElementList { get; }
        IHud Hud { get; }
        IItemList ItemList { get; }
        IParser Parser { get; }
        IActor Player { get; }
        IRandomizer Random { get; }
        ISounds Sounds { get; }
        IState State { get; }
        ITargetList TargetList { get; }
        ITiles Tiles { get; }
        bool TitleScreen { get; }
        IWorld World { get; }
        IActor ActorAt(IXyPair difference);
        int ActorIndexAt(IXyPair location);
        int Adjacent(IXyPair location, int id);
        void Attack(int index, IXyPair location);
        bool BroadcastLabel(int sender, string label, bool force);
        void ClearBoard();
        void ClearSound();
        void ClearWorld();
        void Convey(IXyPair center, int direction);
        void Destroy(IXyPair location);
        AnsiChar Draw(IXyPair location);
        IElement ElementAt(IXyPair location);
        ISound EncodeMusic(string music);
        void EnterBoard();
        void ExecuteCode(int index, IExecutable instructionSource, string name);
        bool ExecuteLabel(int sender, ISearchContext context, string prefix);
        bool ExecuteTransaction(IOopContext context, bool take);
        void FadePurple();
        bool FindTile(ITile kind, IXyPair location);
        void ForcePlayerColor(int index);
        IXyPair GetCardinalVector(int index);
        void HandlePlayerInput(IActor actor);
        void Harm(int index);
        void LoadWorld(string name);
        void LockActor(int index);
        void MoveActor(int index, IXyPair location);
        void MoveActorOnRiver(int index);
        void PlaySound(int priority, ISound sound, int? offset = null, int? length = null);
        void PlotTile(IXyPair location, ITile tile);
        void Push(IXyPair location, IXyPair vector);
        void PushThroughTransporter(IXyPair location, IXyPair vector);
        void PutTile(IXyPair location, IXyPair vector, ITile kind);
        void RaiseError(string error);
        void ReadInput();
        void RemoveActor(int index);
        void RemoveItem(IXyPair location);
        IXyPair Rnd();
        IXyPair RndP(IXyPair vector);
        IXyPair Seek(IXyPair location);
        void SetBoard(int boardIndex);
        void SetEditorMode();
        void SetGameMode();
        void SetMessage(int duration, IMessage message);
        void ShowHelp(string name);
        void ShowInGameHelp();
        void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source);
        bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned);
        void Start();
        void Stop();
        void UnlockActor(int index);
        void UpdateBoard(IXyPair location);
        void UpdateRadius(IXyPair location, RadiusMode mode);
        void UpdateStatus();
        void WaitForTick();
        void ClearForest(IXyPair location);
        void CleanUpPassageMovement();
        IActionList ActionList { get; }
        IDrawList DrawList { get; }
        IInteractionList InteractionList { get; }
        IFacts Facts { get; }
        IHeap Heap { get; }
        IMemory Memory { get; }
        void StepOnce();
        string[] GetMessageLines();
        ITimers Timers { get; }
        IDrumBank DrumBank { get; }
        bool ThreadActive { get; }
        int BaseMemoryUsage { get; }
        void Cheat();
    }
}