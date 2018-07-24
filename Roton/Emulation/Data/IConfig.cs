namespace Roton.Emulation.Data
{
    public interface IConfig
    {
        string DefaultWorld { get; }
        string HomePath { get; }
        int? RandomSeed { get; }
        int AudioSampleRate { get; }
        int AudioDrumRate { get; }
    }
}