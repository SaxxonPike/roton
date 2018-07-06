using System.IO;
using System.Reflection;

namespace Roton.Core
{
    public class ResourceService : IResourceService
    {
        public IResource GetResource(Assembly assembly)
        {
            var name = $"{assembly.GetName().Name}.Resources.resources.zip";
            using (var stream = assembly.GetManifestResourceStream(name))
            using (var mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                return new Resource(mem.ToArray());
            }
        }
    }
}