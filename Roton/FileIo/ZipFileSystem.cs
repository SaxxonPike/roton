using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace Roton.FileIo
{
    public class ZipFileSystem : IFileSystem
    {
        private byte[] _file;

        public ZipFileSystem(byte[] file)
        {
            _file = file;
        }

        public string GetCombinedPath(params string[] paths)
        {
            return string.Join("/", paths);
        }

        public IEnumerable<string> GetDirectoryNames(string path)
        {
            using (var archiveStream = new MemoryStream(_file))
            using (var archive = ZipFile.Read(archiveStream))
            {
                return archive.Entries
                    .Where(e => e.IsDirectory && e.FileName.StartsWith(path) && e.FileName != path)
                    .Select(e => e.FileName.Split('/').Last())
                    .ToList();
            }
        }

        public byte[] GetFile(string filename)
        {
            using (var archiveStream = new MemoryStream(_file))
            using (var archive = ZipFile.Read(archiveStream))
            {
                var entry = archive.Entries.FirstOrDefault(e => e.FileName == filename);
                if (entry == null)
                    return null;

                using (var outputStream = new MemoryStream())
                {
                    entry.Extract(outputStream);
                    outputStream.Flush();
                    return outputStream.ToArray();
                }
            }
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            using (var archiveStream = new MemoryStream(_file))
            using (var archive = ZipFile.Read(archiveStream))
            {
                return archive.Entries
                    .Where(e => !e.IsDirectory && e.FileName.StartsWith(path))
                    .Select(e => e.FileName.Split('/').Last())
                    .ToList();
            }
        }

        public string GetParentPath(string path)
        {
            var paths = path.Split('/').Where(d => !string.IsNullOrEmpty(d)).ToList();
            return paths.Count > 1
                ? string.Join("/", paths.Take(paths.Count - 1))
                : path;
        }

        public void PutFile(string filename, byte[] data)
        {
            using (var archiveStream = new MemoryStream(_file))
            using (var archive = ZipFile.Read(archiveStream))
            {
                archive.AddEntry(filename, data);
                archiveStream.Flush();
                _file = archiveStream.ToArray();
            }
        }
    }
}
