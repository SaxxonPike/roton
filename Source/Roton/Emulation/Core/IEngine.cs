using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IEngine
{
    void Attack(int index, Location location);
    void Destroy(Location location);
    void Harm(int index);
    void PlotTile(Location location, Tile tile);
    void PutTile(Location location, Vector vector, Tile kind);
    void StepOnce();
    void Delay(int msec);
    int ResetBoardTimeHsec();
}