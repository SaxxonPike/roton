using System.Collections.Generic;

namespace Roton.Interface.Audio.Composition
{
    public interface IAudioComposer : ISpeaker
    {
        IEnumerable<int> ComposeAudio();
    }
}
