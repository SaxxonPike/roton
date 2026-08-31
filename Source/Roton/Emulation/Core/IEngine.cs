using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IEngine
{
    event EventHandler Exited;
    event EventHandler Tick;

    bool TitleScreen { get; }
    void Attack(int index, Location location);
    void Destroy(Location location);
    IElement ElementAt(Location location);
    void ExecuteCode(int index, ref Word instruction, string name);
    bool ExecuteTransaction(ref OopContext context, ref Word instruction, bool take);
    void FadePurple();
    bool FindTile(Tile kind, Location location);
    Vector GetCardinalVector(int index);
    void Harm(int index);
    void PlotTile(Location location, Tile tile);
    void PutTile(Location location, Vector vector, Tile kind);
    void RaiseError(ref OopContext oopContext, ReadOnlySpan<char> error);
    void ReadInput(bool isUiFocused);
    void RemoveActor(int index);
    Vector Rnd();
    Vector RndP(Vector vector);
    Vector Seek(Location location);
    void SetMessage(int duration, IMessage message);
    void Start();
    void Stop();
    void WaitForTick();
    void StepOnce();
    bool ThreadActive { get; }
    int MemoryUsage { get; }
    void Cheat();
    void ShowHighScores();
    void Delay(int msec);
    int ResetBoardTimeHsec();
}