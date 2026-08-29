using System;
using Roton.Composers.Audio.Synths;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Drums.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DrumComposer(
    ISynth synth,
    IConfig config)
    : IDrumComposer
{
    private int[] _drumFrequencies = [0];
    private int _drumFrequencyCount = 1;
    private int _drumTimeCounter;
    private int _drumFrequencyIndex;
    private int _drumTime;

    public void SetDrum(ReadOnlySpan<int> frequencies, float rate)
    {
        _drumFrequencyCount = frequencies.Length + 1;
        if (_drumFrequencyCount > _drumFrequencies.Length)
            _drumFrequencies = new int[_drumFrequencyCount];
        frequencies.CopyTo(_drumFrequencies);
        _drumFrequencies[_drumFrequencyCount - 1] = 0;
        synth.SetFrequency(_drumFrequencies[0]);
        _drumTimeCounter = 0;
        _drumTime = (int)Math.Round(config.AudioSampleRate / rate);
    }

    public void ClearDrum()
    {
        _drumFrequencyCount = 0;
        _drumTimeCounter = 0;
        _drumFrequencyIndex = 0;
        synth.SetFrequency(0);
    }

    public int ComposeDrum(Span<float> buffer)
    {
        var remaining = buffer.Length;
        var bufferIdx = 0;

        while (remaining > 0)
        {
            if (_drumFrequencyIndex >= _drumFrequencyCount)
                break;

            if (_drumTimeCounter == 0)
            {
                _drumFrequencyIndex++;

                if (_drumFrequencyIndex >= _drumFrequencyCount)
                    break;

                _drumTimeCounter = _drumTime;
                synth.SetFrequency(_drumFrequencies[_drumFrequencyIndex]);
            }

            var toCompose = Math.Min(remaining, _drumTimeCounter);
            var rendered = synth.Render(buffer.Slice(bufferIdx, toCompose));
            if (rendered <= 0)
                break;

            remaining -= rendered;
            _drumTimeCounter -= rendered;
            bufferIdx += rendered;
        }

        return bufferIdx;
    }
}