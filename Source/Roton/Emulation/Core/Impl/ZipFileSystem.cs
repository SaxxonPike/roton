using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public sealed class ZipFileSystem : IFileSystem
    {
        private byte[] _file;

        public ZipFileSystem(byte[] file)
        {
            if (file == null || file.Length == 0)
                throw new Exception($"Can't load {nameof(ZipFileSystem)} source.");
            _file = file;
        }

        public string GetCombinedPath(params string[] paths)
        {
            return string.Join("/", paths);
        }

        public IEnumerable<string> GetDirectoryNames(string path)
        {
            using var archiveStream = new MemoryStream(_file);
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
            return archive.Entries
                .Where(e => e.FullName.StartsWith(path) && e.FullName != path)
                .Select(e => e.FullName.Split('/').Last())
                .ToList();
        }

        // TODO: make zips writeable
        public bool IsWriteable => false;

        public bool FileExists(string path)
        {
            using var archiveStream = new MemoryStream(_file);
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
            return archive.Entries.Any(e => e.FullName.Equals(path));
        }

        public byte[] GetFile(string filename)
        {
            using var archiveStream = new MemoryStream(_file);
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
            var entry = archive.Entries.FirstOrDefault(e => e.FullName == filename);
            if (entry == null)
                return null;

            using var outputStream = new MemoryStream();
            using var inputStream = entry.Open();
            inputStream.CopyTo(outputStream);
            outputStream.Flush();
            return outputStream.ToArray();
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            using var archiveStream = new MemoryStream(_file);
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
            return archive.Entries
                .Where(e => e.FullName.StartsWith(path))
                .Select(e => e.FullName.Split('/').Last())
                .ToList();
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
            using var archiveStream = new MemoryStream(_file);
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Update);
            var entry = archive.CreateEntry(filename, CompressionLevel.Optimal);
            using (var outputStream = entry.Open())
            {
                using var inputStream = new MemoryStream(data);
                inputStream.CopyTo(outputStream);
                outputStream.Flush();
            }

            archiveStream.Flush();
            _file = archiveStream.ToArray();
        }
    }
}