using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IRandomizer
{
    void Initialize();
    int GetNext(int exclusiveUpperBound);
    int Seed { get; set; }
    int State { get; set; }
}