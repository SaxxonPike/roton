using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public sealed class AudioConfig
{
    public int SampleRate { get; set; } = 44100;
    public int DrumRate { get; set; } = 64;
    public int BufferSize { get; set; } = 2048;
}

public sealed class JoystickConfig
{
    public float DeadZone { get; set; }
    public float DenoiseZone { get; set; }
    public bool Disable { get; set; }
}

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