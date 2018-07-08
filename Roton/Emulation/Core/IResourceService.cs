using System.Reflection;

namespace Roton.Emulation.Core
{
    public interface IResourceService
    {
        IResource GetResource(Assembly assembly);
    }
}