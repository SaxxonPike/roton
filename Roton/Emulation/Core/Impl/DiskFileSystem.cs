using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public class DiskFileSystem : IFileSystem
    {
        private readonly string _basePath;

        public DiskFileSystem() : this(Environment.CurrentDirectory)
        {
        }

        public DiskFileSystem(string basePath)
        {
            _basePath = basePath;
        }

        public byte[] GetFile(string path)
        {
            return File.ReadAllBytes(Path.IsPathRooted(path)
                ? path
                : Path.Combine(_basePath, path));
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            return Directory.GetFiles(Path.IsPathRooted(path)
                ? path
                : Path.Combine(_basePath, path))
                .Select(p => new FileInfo(p).Name);
        }

        public void PutFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }
    }
}