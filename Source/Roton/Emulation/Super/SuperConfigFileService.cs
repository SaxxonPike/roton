using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperConfigFileService : IConfigFileService
{
    public IConfigFile? Load()
    {
        return null;
    }

    public void Save(IConfigFile configFile)
    {
    }
}