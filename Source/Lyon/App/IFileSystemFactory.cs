using Roton.Emulation.Core;

namespace Lyon.App;

/// <summary>
/// Create <see cref="IFileSystem"/> instances.
/// </summary>
public interface IFileSystemFactory
{
    /// <summary>
    /// Creates a <see cref="IFileSystem"/> instance using the specified path as root.
    /// </summary>
    IFileSystem Create(string path);
}