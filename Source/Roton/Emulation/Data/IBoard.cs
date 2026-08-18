using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public interface IBoard
{
    ref Location16 Camera { get; }
    ref Location Entrance { get; }
    IExits Exits { get; }
    bool IsDark { get; set; }
    int MaximumShots { get; set; }
    string Name { get; set; }
    bool RestartOnZap { get; set; }
    int TimeLimit { get; set; }
}