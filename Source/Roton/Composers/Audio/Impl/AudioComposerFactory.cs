using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class AudioComposerFactory(
    IDrumSoundList drumBank,
    IConfig config) : IAudioComposerFactory
{
    public IAudioComposer Get()
    {
        return new AudioComposer(drumBank, config);
    }
}