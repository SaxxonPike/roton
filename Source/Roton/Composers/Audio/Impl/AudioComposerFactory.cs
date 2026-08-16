using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Composers.Audio.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class AudioComposerFactory(
    IDrumBank drumBank,
    IConfig config) : IAudioComposerFactory
{
    public IAudioComposer Get()
    {
        return new AudioComposer(drumBank, config);
    }
}