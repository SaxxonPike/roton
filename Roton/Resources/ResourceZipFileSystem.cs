using System.IO;
using Roton.FileIo;

namespace Roton.Resources
{
    public class ResourceZipFileSystem : IResourceZipFileSystem
    {
        private byte[] _data;

        public ResourceZipFileSystem()
        {
            using (var stream = typeof(ResourceZipFileSystem).Assembly.GetManifestResourceStream("Resources/resources.zip"))
            using (var mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                _data = mem.ToArray();
            }
        }
        
        public IFileSystem Root => GetPrependedFileSystem("root/");
        public IFileSystem System => GetPrependedFileSystem("system/");

        private IFileSystem GetPrependedFileSystem(string path)
        {
            return new PrependedFileSystem(new ZipFileSystem(_data), path);
        }
    }

    public interface IResourceZipFileSystem
    {
        IFileSystem Root { get; }
        IFileSystem System { get; }
    }
}