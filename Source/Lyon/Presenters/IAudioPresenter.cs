using Roton.Composers.Audio;

namespace Lyon.Presenters;

public interface IAudioPresenter
{
    float Volume { get; set; }
    void Start();
    void Stop();
    void Update(AudioComposerDataEventArgs buffer);
    int SampleRate { get; }
}