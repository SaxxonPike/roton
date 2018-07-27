using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Composers.Audio.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public class AudioComposerFactory : IAudioComposerFactory
    {
        private readonly Lazy<IDrumBank> _drumBank;
        private readonly Lazy<IConfig> _config;

        public AudioComposerFactory(
            Lazy<IDrumBank> drumBank, 
            Lazy<IConfig> config)
        {
            _drumBank = drumBank;
            _config = config;
        }
        
        public IAudioComposer Get()
        {
            return new AudioComposer(_drumBank, _config.Value);
        }
    }
}