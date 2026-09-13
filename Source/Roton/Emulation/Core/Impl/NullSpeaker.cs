using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class NullSpeaker : ISpeaker
{
    public void PlayDrum(int drum)
    {
    }

    public void PlayNote(int note)
    {
    }

    public void PlayStep()
    {
    }

    public void Tick()
    {
    }

    public void StopNote()
    {
    }
}