using Roton.Composers.Audio;

namespace Lyon.Presenters;

public interface IAudioPresenter
{
    double Volume { get; set; }
    void Start();
    void Stop();
    void Update(AudioComposerDataEventArgs buffer);
    int SampleRate { get; }
}