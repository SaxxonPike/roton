using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton
{
    public class FileSystem : IFileSystem
    {
        public FileSystem()
        {
            this.CurrentDirectory = Environment.CurrentDirectory;
        }

        public void ChangeDirectory(string relativeDirectory)
        {
            this.CurrentDirectory = Path.Combine(this.CurrentDirectory, relativeDirectory);
        }

        public string CurrentDirectory
        {
            get;
            set;
        }

        public IList<string> GetDirectories()
        {
            var result = Directory.GetDirectories(this.CurrentDirectory)
                .Select<string, string>(i => Path.GetFileName(i)).ToList();
            return result;
        }

        public IList<string> GetFiles()
        {
            var result = Directory.GetFiles(this.CurrentDirectory)
                .Select<string, string>(i => Path.GetFileName(i)).ToList();
            return result;
        }

        public byte[] ReadFile(string filename)
        {
            return File.ReadAllBytes(Path.Combine(this.CurrentDirectory, filename));
        }

        public void WriteFile(string filename, byte[] data)
        {
            File.WriteAllBytes(Path.Combine(this.CurrentDirectory, filename), data);
        }
    }
}
