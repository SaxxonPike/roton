namespace Roton.Emulation.Core;

public interface IRandomState
{
    int Seed { get; }
    int State { get; set; }
}