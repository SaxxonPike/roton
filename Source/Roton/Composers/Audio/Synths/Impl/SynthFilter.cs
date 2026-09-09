using Roton.Infrastructure;

namespace Roton.Composers.Audio.Synths.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class SynthFilter : ISynthFilter
{
    /// <inheritdoc />
    public float PolyBlep(float halfPhase, float halfPhasePerSample)
    {
        if (halfPhase < halfPhasePerSample)
        {
            halfPhase /= halfPhasePerSample;
            return halfPhase + halfPhase - halfPhase * halfPhase - 1f;
        }

        if (halfPhase > 1f - halfPhasePerSample)
        {
            halfPhase = (halfPhase - 1f) / halfPhasePerSample;
            return halfPhase * halfPhase + halfPhase + halfPhase + 1f;
        }

        return 0f;
    }

    /// <inheritdoc />
    public float LowPass(float raw, float halfPhasePerSample, float filterState, out float resultFilterState)
    {
        var alpha = 1f - halfPhasePerSample;
        resultFilterState = alpha * (raw - filterState);
        return filterState + resultFilterState;
    }
}