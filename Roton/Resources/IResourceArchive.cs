﻿namespace Roton.Resources
{
    public interface IResourceArchive
    {
        byte[] GetElementData();
        byte[] GetMemoryData();
    }
}