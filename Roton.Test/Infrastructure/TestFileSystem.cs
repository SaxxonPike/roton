using System.Collections.Generic;
using Roton.Emulation.Core;

namespace Roton.Test.Infrastructure
{
    public class TestFileSystem : IFileSystem
    {
        public byte[] GetFile(string path)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            throw new System.NotImplementedException();
        }

        public void PutFile(string path, byte[] data)
        {
            throw new System.NotImplementedException();
        }
    }
}