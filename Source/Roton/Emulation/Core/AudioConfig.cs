namespace Roton.Emulation.Core;

public sealed class AudioConfig
{
    /// <summary>
    /// Sample rate in Hz.
    /// </summary>
    public int SampleRate { get; set; } = 44100;

    /// <summary>
    /// Number of drum frequencies processed per second.
    /// </summary>
    public int DrumSpeed { get; set; } = 64;

    /// <summary>
    /// If true, a cutoff filter is enabled and applied to the sound output.
    /// </summary>
    public bool LowPass { get; set; } = true;

    /// <summary>
    /// Cutoff frequency of the low-pass filter in Hz.
    /// </summary>
    public float LowPassCutoff { get; set; } = 12000f;

    /// <summary>
    /// If true, interpolation is enabled and applied to the sound output.
    /// </summary>
    public bool Interpolate { get; set; } = true;

    /// <summary>
    /// Input gain of the speaker audio stream.
    /// </summary>
    public float PreGain { get; set; } = 0.5f;

    /// <summary>
    /// Output gain of the speaker audio stream.
    /// </summary>
    public float Gain { get; set; } = 0.14f;
}