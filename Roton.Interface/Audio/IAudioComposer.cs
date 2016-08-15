using System.Collections.Generic;

namespace Roton.Interface.Audio
{
    public interface IAudioComposer
    {
        IEnumerable<int> ComposeAudio();
        void PlayDrum(int index);
        void PlayNote(int note);
    }
}
