namespace Roton.Interface.Audio.Presentation
{
    public interface IAudioPresenter
    {
        double Volume { get; set; }
        void Start();
        void Stop();
    }
}