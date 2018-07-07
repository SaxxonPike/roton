using System.Reflection;

namespace Roton.Core
{
    public interface IResourceService
    {
        IResource GetResource(Assembly assembly);
    }
}