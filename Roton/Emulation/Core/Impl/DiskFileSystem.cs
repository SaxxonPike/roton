using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public sealed class DiskFileSystem : IFileSystem
    {
        private readonly string _basePath;

        public DiskFileSystem() : this(Environment.CurrentDirectory)
        {
        }

        public DiskFileSystem(string basePath)
        {
            _basePath = !string.IsNullOrWhiteSpace(basePath)
                ? basePath
                : Environment.CurrentDirectory;
        }

        public bool IsWriteable => true;

        private string AdjustPath(string path) => Path.IsPathRooted(path)
                                                      ? path
                                                      : Path.Combine(_basePath, path);

        public bool FileExists(string path) => File.Exists(GetBestMatch(AdjustPath(path)));

        /// <summary>
        /// Selects the nearest match for a specified file in a case-sensitive file system.
        /// </summary>
        /// <param name="path">The path that should be evaluated.</param>
        /// <returns>The adjusted path, based on which files were found.</returns>
        private string GetBestMatch(string path)
        {
            // This function follows a few simple rules:
            //    1) Directory names are ignored. Those are assumed to be specified
            //       correctly.
            //    2) If an exact file match is found, use that.
            //    3) If the file list contains an inexact match (i.e. the case doesn't
            //       match), use the first one it comes across.
            //    4) If a match could not be found, return the string that the function
            //       called with. It's the responsibility of the calling function to
            //       handle the error.
            
            // Rule #2:
            if(File.Exists(path)) return path;
            
            // Rule #3:
            var directory = Path.GetDirectoryName(path) ?? ".";
            var filename = Path.GetFileName(path)?.ToLower();

            var fileList = Directory.GetFiles(directory);
            foreach(var file in fileList.Select(Path.GetFileName)) {
                if(file.ToLower() == filename)
                    return Path.Combine(directory, file);
            }

            // Rule #4:
            return path;
        }

        public byte[] GetFile(string path) => File.ReadAllBytes(GetBestMatch(AdjustPath(path)));

        public IEnumerable<string> GetFileNames(string path) =>
            Directory.GetFiles(AdjustPath(path)).Select(p => new FileInfo(p).Name);

        public void PutFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }
    }
}