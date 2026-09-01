using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFeatures
{
    string[] GetMessageLines();
    int BaseMemoryUsage { get; }
    void CleanUpOop(ref OopContext context);
}