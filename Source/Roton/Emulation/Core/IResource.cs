﻿namespace Roton.Emulation.Core
{
    public interface IResource
    {
        IFileSystem Root { get; }
        IFileSystem System { get; }
    }
}