namespace Roton.Core
{
    public interface IBoard
    {
        IXyPair Camera { get; }
        IXyPair Enter { get; }
        bool Dark { get; set; }
        int ExitEast { get; set; }
        int ExitNorth { get; set; }
        int ExitSouth { get; set; }
        int ExitWest { get; set; }
        string Name { get; set; }
        bool RestartOnZap { get; set; }
        int Shots { get; set; }
        int TimeLimit { get; set; }
    }
}