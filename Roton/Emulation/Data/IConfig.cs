namespace Roton.Emulation.Data
{
    public interface IConfig
    {
        string DefaultWorld { get; }
        int? RandomSeed { get; }
    }
}