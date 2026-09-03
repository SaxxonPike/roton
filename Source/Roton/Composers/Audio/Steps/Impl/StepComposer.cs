using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Steps.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class StepComposer(
    IConfig config)
    : IStepComposer
{
    private int _stepCounter;

    public void SetStep() =>
        _stepCounter = (int)Math.Round(config.AudioSampleRate / 22050f) + 1;

    public void ClearStep() =>
        _stepCounter = 0;

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