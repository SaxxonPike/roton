namespace Roton.Emulation.Data
{
    public interface IBoard
    {
        IXyPair Camera { get; }
        IXyPair Entrance { get; }
        int ExitEast { get; set; }
        int ExitNorth { get; set; }
        int ExitSouth { get; set; }
        int ExitWest { get; set; }
        bool IsDark { get; set; }
        int MaximumShots { get; set; }
        string Name { get; set; }
        bool RestartOnZap { get; set; }
        int TimeLimit { get; set; }
    }
}