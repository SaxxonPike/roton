namespace Lyon.Presenters
{
    public interface IAudioPresenter
    {
        double Volume { get; set; }
        void Start();
        void Stop();
    }
}