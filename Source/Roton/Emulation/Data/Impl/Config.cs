namespace Roton.Emulation.Data.Impl
{
    public class Config : IConfig
    {
        public string DefaultWorld { get; set; }
        public string HomePath { get; set; }
        public int? RandomSeed { get; set; }
        public int AudioSampleRate { get; set; }
        public int AudioDrumRate { get; set; }
        public int AudioBufferSize { get; set; }
        public int VideoScale { get; set; }
        public int MasterClockNumerator { get; set; }
        public int MasterClockDenominator { get; set; }
        public bool FastMode { get; set; }
    }
}