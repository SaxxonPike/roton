﻿using System.Collections.Generic;
using Roton.FileIo;

namespace Roton.Resources
{
    public class ResourceZipFileSystem : ZipFileSystem, IResourceArchive
    {
        public ResourceZipFileSystem(byte[] file) : base(file)
        {
        }

        public byte[] GetZztElementData()
        {
            return GetFile(GetCombinedPath("system", "elements-zzt.bin"));
        }

        public byte[] GetSuperZztElementData()
        {
            return GetFile(GetCombinedPath("system", "elements-szzt.bin"));
        }

        public byte[] GetZztMemoryData()
        {
            return GetFile(GetCombinedPath("system", "memory-zzt.bin"));
        }

        public byte[] GetSuperZztMemoryData()
        {
            return GetFile(GetCombinedPath("system", "memory-szzt.bin"));
        }

        public IEnumerable<string> GetRootFileNames()
        {
            return GetFileNames("root");
        }
    }
}