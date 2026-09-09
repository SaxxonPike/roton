using Roton.Emulation.Data;

namespace Lyon.Common.App.Impl;

public sealed class Config : IConfig
{
    public string? DefaultWorld { get; set; }
    public string? HomePath { get; set; }
    public int? RandomSeed { get; set; }
    public int AudioSampleRate { get; set; } = 44100;
    public int AudioDrumRate { get; set; } = 64;
    public int AudioBufferSize { get; set; } = 2048;
    public int MasterClockNumerator { get; set; } = 100;
    public int MasterClockDenominator { get; set; } = 7275;
    public bool FastMode { get; set; }
    public bool TraceOop { get; set; }
    public bool NoPesterMode { get; set; }
    public float JoystickDeadZone { get; set; } = 0.5f;
    public float JoystickDenoiseZone { get; set; } = 0.1f;
    public bool DisableJoystick { get; set; }
    public bool SkipIntro { get; set; }
}