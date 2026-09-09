namespace Roton.Emulation.Core;

public sealed class AudioConfig
{
    public int SampleRate { get; set; } = 44100;
    public int DrumRate { get; set; } = 64;
    public int BufferSize { get; set; } = 2048;
}