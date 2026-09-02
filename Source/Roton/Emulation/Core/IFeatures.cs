using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFeatures
{
    void CleanUpOop(ref OopContext context);
}