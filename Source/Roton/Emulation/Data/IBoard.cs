using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public interface IBoard
{
    ref Location16 Camera { get; }
    ref Location Entrance { get; }
    IExits Exits { get; }
    ref Bool IsDark { get; }
    ref Word MaximumShots { get; }
    string Name { get; set; }
    ref Bool RestartOnZap { get; }
    ref Word TimeLimit { get; }
}