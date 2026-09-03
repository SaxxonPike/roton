namespace Roton.Emulation.Data.Impl;

internal sealed class HighScore : IHighScore
{
    public string? Name { get; set; }
    public int Score { get; set; }
}