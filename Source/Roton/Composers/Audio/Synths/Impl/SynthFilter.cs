using Roton.Infrastructure;

namespace Roton.Composers.Audio.Synths.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class SynthFilter : ISynthFilter
{
    /// <inheritdoc />
    public float PolyBlep(float t, float dt)
    {
        if (t < dt)
        {
            t /= dt;
            return t + t - t * t - 1f;
        }

        if (t > 1f - dt)
        {
            t = (t - 1f) / dt;
            return t * t + t + t + 1f;
        }

        return 0f;
    }

    /// <inheritdoc />
    public float LowPass(float x, float halfPhasePerSample, float filterState, out float resultFilterState)
    {
        var alpha = 1f - halfPhasePerSample;
        resultFilterState = filterState + alpha * (x - filterState);
        return resultFilterState;
    }
}