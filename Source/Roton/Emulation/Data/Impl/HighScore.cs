namespace Roton.Emulation.Data.Impl;

internal sealed class HighScore : IHighScore
{
    internal HighScore()
    {
    }
        
    public string? Name { get; set; }
    public int Score { get; set; }
}