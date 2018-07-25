using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public class Config : IConfig
    {
        public string DefaultWorld { get; set; }
        public string HomePath { get; set; }
        public int? RandomSeed { get; set; }
        public int AudioSampleRate { get; set; }
        public int AudioDrumRate { get; set; }
    }
}