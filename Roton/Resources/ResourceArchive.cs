namespace Roton.Resources
{
    public class ResourceArchive : IResourceArchive
    {
        private readonly IResourceZipFileSystem _resourceZipFileSystem;

        public ResourceArchive(IResourceZipFileSystem resourceZipFileSystem)
        {
            _resourceZipFileSystem = resourceZipFileSystem;
        }
        
        public byte[] GetSuperZztElementData()
        {
            return _resourceZipFileSystem.System.GetFile("elements-szzt.bin");
        }

        public byte[] GetSuperZztMemoryData()
        {
            return _resourceZipFileSystem.System.GetFile("memory-szzt.bin");
        }

        public byte[] GetZztElementData()
        {
            return _resourceZipFileSystem.System.GetFile("elements-zzt.bin");
        }

        public byte[] GetZztMemoryData()
        {
            return _resourceZipFileSystem.System.GetFile("memory-zzt.bin");
        }
    }
}