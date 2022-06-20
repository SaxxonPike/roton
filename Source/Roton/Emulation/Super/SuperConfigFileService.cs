using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperConfigFileService : IConfigFileService
{
    public IConfigFile Load()
    {
        return null;
    }

    public void Save(IConfigFile configFile)
    {
    }
}