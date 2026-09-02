using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IEngine
{
    void Attack(int index, Location location);
    void Destroy(Location location);
    void ExecuteCode(int index, ref Word instruction, string name);
    void Harm(int index);
    void PlotTile(Location location, Tile tile);
    void PutTile(Location location, Vector vector, Tile kind);
    void RaiseError(ref OopContext oopContext, ReadOnlySpan<char> error);
    void StepOnce();
    void Cheat();
    void Delay(int msec);
    int ResetBoardTimeHsec();
}