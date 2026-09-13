namespace Roton.Composers.Audio.AudioStreams;

/// <summary>
/// Represents a tone played by the speaker.
/// </summary>
/// <param name="frequency">
/// Frequency of the tone, in Hz.
/// </param>
/// <param name="duration">
/// Duration of the tone, in seconds.
/// </param>
public readonly struct SpeakerTone(float frequency, float duration)
{
    public float Frequency { get; } = frequency;
    public float Duration { get; } = duration;
}