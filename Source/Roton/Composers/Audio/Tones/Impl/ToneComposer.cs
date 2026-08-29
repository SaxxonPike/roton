using System;
using Roton.Composers.Audio.Synths;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Tones.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ToneComposer(
    ISynth synth
) : IToneComposer
{
    /// <summary>
    /// Reference frequency.
    /// </summary>
    private const double RootFrequency = 440d;

    /// <summary>
    /// Root note corresponding to the reference frequency.
    /// </summary>
    private const int RootNote = 45;

    /// <inheritdoc />
    public void SetTone(int toneNumber) =>
        synth.SetFrequency((float)(RootFrequency * Math.Pow(2d, (toneNumber - RootNote) / 12d)));

    /// <inheritdoc />
    public void ClearTone() =>
        synth.SetFrequency(0);

    /// <inheritdoc />
    public int ComposeTone(Span<float> buffer) =>
        synth.Render(buffer);
}