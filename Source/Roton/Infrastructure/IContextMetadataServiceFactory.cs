using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure
{
    public interface IContextMetadataServiceFactory
    {
        IContextMetadataService Get(Context context);
    }
}