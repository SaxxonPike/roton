using System.Collections.Generic;

namespace Lyon.Presenters
{
    public interface IAudioPresenter
    {
        double Volume { get; set; }
        void Start();
        void Stop();
        void Update(IEnumerable<float> buffer);
        int SampleRate { get; }
    }
}