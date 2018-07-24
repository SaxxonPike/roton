using System;
using Roton.Emulation.Data;

namespace Roton.Composers.Audio.Impl
{
    public class AudioComposerFactory : IAudioComposerFactory
    {
        private readonly Lazy<IDrumBank> _drumBank;
        private readonly Lazy<IConfig> _config;

        public AudioComposerFactory(Lazy<IDrumBank> drumBank, Lazy<IConfig> config)
        {
            _drumBank = drumBank;
            _config = config;
        }
        
        public IAudioComposer Get()
        {
            return new AudioComposer(_drumBank.Value, _config.Value);
        }
    }
}