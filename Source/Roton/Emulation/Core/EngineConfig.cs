namespace Roton.Emulation.Core;

public sealed class EngineConfig
{
    public string? DefaultWorld { get; set; }
    public string? HomePath { get; set; }
    public int? RandomSeed { get; set; }
    public int MasterClockNumerator { get; set; }
    public int MasterClockDenominator { get; set; }
    public bool FastMode { get; set; }
    public bool NoPesterMode { get; set; }
    public bool SkipIntro { get; set; }
}