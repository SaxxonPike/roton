using System.Collections.Generic;

namespace Roton.Interface.Audio
{
    public interface IAudioComposer
    {
        IEnumerable<int> ComposeAudio(int numberOfSamples);
        void PlayDrum(int index);
        void PlayTone(int note);
    }
}
