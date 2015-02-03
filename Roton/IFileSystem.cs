using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface IFileSystem
    {
        void ChangeDirectory(string relativeDirectory);
        IList<string> GetDirectories();
        IList<string> GetFiles();
        byte[] ReadFile(string filename);
        void WriteFile(string filename, byte[] data);
    }
}
