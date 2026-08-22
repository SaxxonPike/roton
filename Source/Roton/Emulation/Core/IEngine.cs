using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IEngine
{
    event EventHandler Exited;
    event EventHandler Tick;

    bool TitleScreen { get; }
    IActor ActorAt(Location location);
    int ActorIndexAt(Location location);
    int Adjacent(Location location, int id);
    void Attack(int index, Location location);
    bool BroadcastLabel(int sender, ReadOnlySpan<char> label, bool ignoreLock);
    void CleanUpOop(ref OopContext context);
    void ClearBoard();
    void ClearSound();
    void ClearWorld();
    void Convey(Location center, int direction);
    void Destroy(Location location);
    AnsiChar Draw(Location location);
    IElement ElementAt(Location location);
    void EnterBoard();
    void ExecuteCode(int index, ref Word instruction, string name);
    bool ExecuteLabel(int sender, ref SearchContext search, ReadOnlySpan<char> term, ReadOnlySpan<char> prefix);
    bool ExecuteTransaction(ref OopContext context, ref Word instruction, bool take);
    void FadePurple();
    bool FindTile(Tile kind, Location location);
    void ForcePlayerColor(int index);
    Vector GetCardinalVector(int index);
    int GetColorMatchValue(int color);
    void HandlePlayerInput(IActor actor);
    void Harm(int index);
    bool LoadWorld(string name, bool savedGame);
    void LockActor(int index);
    void MoveActor(int index, Location location);
    void MoveActorOnRiver(int index);
    void NotifyActorSentLabel(int index);
    void PlaySound(int priority, ISound sound, int? offset = null, int? length = null);
    void PlotTile(Location location, Tile tile);
    void Push(Location location, Vector vector);
    void PushThroughTransporter(Location location, Vector vector);
    void PutTile(Location location, Vector vector, Tile kind);
    void RaiseError(ref OopContext oopContext, ReadOnlySpan<char> error);
    void ReadInput(bool isUiFocused);
    void RemoveActor(int index);
    void RemoveItem(Location location);
    Vector Rnd();
    Vector RndP(Vector vector);
    void SaveWorld(string name);
    Vector Seek(Location location);
    void SetBoard(int boardIndex);
    void SetEditorMode();
    void SetGameMode();
    void SetMessage(int duration, IMessage message);
    void ShowHelp(string title, string filename);
    void ShowInGameHelp();
    void OpenWorld();
    bool RestoreWorld();
    void SpawnActor(Location location, Tile tile, int cycle, IActor? source);
    bool SpawnProjectile(int id, Location location, Vector vector, bool enemyOwned);
    void Start();
    void Stop();
    void UnlockActor(int index);
    void UpdateBoard(Location location);
    void UpdateRadius(Location location, RadiusMode mode);
    void UpdateStatus();
    void WaitForTick();
    void ClearForest(Location location);
    void CleanUpPassageMovement();
    void StepOnce();
    string[] GetMessageLines();
    bool ThreadActive { get; }
    int MemoryUsage { get; }
    void Cheat();
    void PlayStep();
    string GetHighScoreName(string fileName);
    void ShowHighScores();
    string? ShowLoad(string title, string extension);
    void PackBoard();
    void UnpackBoard(int index);
    void Delay(int msec);
    void PlayErrorSound();
    int ResetBoardTimeHsec();
}