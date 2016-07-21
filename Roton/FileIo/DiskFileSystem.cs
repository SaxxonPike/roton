using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roton.Core;

namespace Roton.FileIo
{
    public class DiskFileSystem : IFileSystem
    {
        public string GetCombinedPath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public IEnumerable<string> GetDirectoryNames(string path)
        {
            return Directory.GetDirectories(Path.IsPathRooted(path)
                ? path
                : Path.Combine(Environment.CurrentDirectory, path))
                .Select(p => new DirectoryInfo(p).Name);
        }

        public byte[] GetFile(string path)
        {
            return File.ReadAllBytes(Path.IsPathRooted(path)
                ? path
                : Path.Combine(Environment.CurrentDirectory, path));
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            return Directory.GetFiles(Path.IsPathRooted(path)
                ? path
                : Path.Combine(Environment.CurrentDirectory, path))
                .Select(p => new FileInfo(p).Name);
        }

        public string GetParentPath(string path)
        {
            return new DirectoryInfo(path).Parent?.Name ?? path;
        }

        public void PutFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }
    }
}
