using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IEngine
{
    event EventHandler Exited;
    event EventHandler Tick;

    bool TitleScreen { get; }
    void Attack(int index, Location location);
    bool BroadcastLabel(int sender, ReadOnlySpan<char> label, bool ignoreLock);
    void Convey(Location center, int direction);
    void Destroy(Location location);
    AnsiChar Draw(Location location);
    IElement ElementAt(Location location);
    void ExecuteCode(int index, ref Word instruction, string name);
    bool ExecuteLabel(int sender, ref SearchContext search, ReadOnlySpan<char> term, ReadOnlySpan<char> prefix);
    bool ExecuteTransaction(ref OopContext context, ref Word instruction, bool take);
    void FadePurple();
    bool FindTile(Tile kind, Location location);
    Vector GetCardinalVector(int index);
    void Harm(int index);
    void MoveActor(int index, Location location);
    void MoveActorOnRiver(int index);
    void PlotTile(Location location, Tile tile);
    void Push(Location location, Vector vector);
    void PushThroughTransporter(Location location, Vector vector);
    void PutTile(Location location, Vector vector, Tile kind);
    void RaiseError(ref OopContext oopContext, ReadOnlySpan<char> error);
    void ReadInput(bool isUiFocused);
    void RemoveActor(int index);
    Vector Rnd();
    Vector RndP(Vector vector);
    Vector Seek(Location location);
    void SetEditorMode();
    void SetGameMode();
    void SetMessage(int duration, IMessage message);
    void ShowHelp(string title, string filename);
    void SpawnActor(Location location, Tile tile, int cycle, IActor? source);
    bool SpawnProjectile(int id, Location location, Vector vector, bool enemyOwned);
    void Start();
    void Stop();
    void UpdateBoard(Location location);
    void UpdateRadius(Location location, RadiusMode mode);
    void UpdateStatus();
    void WaitForTick();
    void StepOnce();
    bool ThreadActive { get; }
    int MemoryUsage { get; }
    void Cheat();
    void ShowHighScores();
    void Delay(int msec);
    int ResetBoardTimeHsec();
}