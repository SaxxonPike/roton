using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Roton
{
    public class FileSystem : IFileSystem
    {
        public FileSystem()
        {
            CurrentDirectory = Environment.CurrentDirectory;
        }

        public void ChangeDirectory(string relativeDirectory)
        {
            CurrentDirectory = Path.Combine(CurrentDirectory, relativeDirectory);
        }

        public string CurrentDirectory { get; set; }

        public IList<string> GetDirectories()
        {
            var result = Directory.GetDirectories(CurrentDirectory)
                .Select(Path.GetFileName).ToList();
            return result;
        }

        public IList<string> GetFiles()
        {
            var result = Directory.GetFiles(CurrentDirectory)
                .Select(Path.GetFileName).ToList();
            return result;
        }

        public byte[] ReadFile(string filename)
        {
            return File.ReadAllBytes(Path.Combine(CurrentDirectory, filename));
        }

        public void WriteFile(string filename, byte[] data)
        {
            File.WriteAllBytes(Path.Combine(CurrentDirectory, filename), data);
        }
    }
}