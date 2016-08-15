using System.Collections.Generic;
using Roton.Core;

namespace Roton.Interface.Audio
{
    public class AudioComposer : IAudioComposer
    {
        private readonly IDrumBank _drumBank;

        public AudioComposer(IDrumBank drumBank, int outputSampleRate, int samplesPerDrumFrequency)
        {
            _drumBank = drumBank;
        }

        public IEnumerable<int> ComposeAudio(int numberOfSamples)
        {
            throw new System.NotImplementedException();
        }

        public void PlayDrum(int index)
        {
            throw new System.NotImplementedException();
        }

        public void PlayTone(int note)
        {
            throw new System.NotImplementedException();
        }
    }
}