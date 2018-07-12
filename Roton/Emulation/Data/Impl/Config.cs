using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public class Config : IConfig
    {
        public string DefaultWorld { get; set; }
        public int? RandomSeed { get; set; }
    }
}