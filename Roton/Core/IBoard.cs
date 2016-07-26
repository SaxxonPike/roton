namespace Roton.Core
{
    public interface IBoard
    {
        IXyPair Camera { get; }
        IXyPair Entrance { get; }
        bool IsDark { get; set; }
        int ExitEast { get; set; }
        int ExitNorth { get; set; }
        int ExitSouth { get; set; }
        int ExitWest { get; set; }
        string Name { get; set; }
        bool RestartOnZap { get; set; }
        int MaximumShots { get; set; }
        int TimeLimit { get; set; }
    }
}