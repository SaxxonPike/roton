using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class AudioComposerFactory(
    IConfig config,
    ISynth synth)
    : IAudioComposerFactory
{
    public IAudioComposer Create() => 
        new AudioComposer(config, synth);
}