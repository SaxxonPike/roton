﻿using System.Collections.Generic;

namespace Roton.Core
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