namespace Roton.Emulation.Data;

public interface IBoard
{
    IXyPair Camera { get; }
    IXyPair Entrance { get; }
    IExits Exits { get; }
    bool IsDark { get; set; }
    int MaximumShots { get; set; }
    string Name { get; set; }
    bool RestartOnZap { get; set; }
    int TimeLimit { get; set; }
}