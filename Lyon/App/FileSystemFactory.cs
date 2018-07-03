using System.Collections.Generic;
using System.IO;
using Roton.FileIo;
using Roton.Interface.Resources;
using Roton.Resources;

namespace Lyon.App
{
    public class FileSystemFactory : IFileSystemFactory
    {
        public IFileSystem Create(string path, string defaultWorld)
        {
            var files = new Dictionary<string, byte[]>
            {
                ["ZZT.CFG"] = GetBytes(defaultWorld)
            };
            
            return new FixedFileSystem(files, new DiskFileSystem(path));
        }

        private byte[] GetBytes(string contents)
        {
            using (var output = new MemoryStream())
            using (var config = new StreamWriter(output))
            {
                config.Write(contents);
                config.Flush();
                return output.ToArray();
            }            
        }
    }
}