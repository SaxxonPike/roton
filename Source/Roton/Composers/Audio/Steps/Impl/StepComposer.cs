using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Steps.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class StepComposer(
    IConfig config)
    : IStepComposer
{
    /// <summary>
    /// Remaining number of active samples.
    /// </summary>
    private int _stepCounter;

    /// <inheritdoc />
    public void SetStep() =>
        _stepCounter = (int)Math.Round(config.Audio.SampleRate / 22050f) + 1;

    /// <inheritdoc />
    public void ClearStep() =>
        _stepCounter = 0;

    /// <inheritdoc />
    public int ComposeStep(Span<float> buffer)
    {
        if (_stepCounter == 0)
            return 0;

        var len = Math.Min(buffer.Length, _stepCounter);
        buffer.Slice(0, len).Fill(1f);
        _stepCounter -= len;
        return len;
    }
}