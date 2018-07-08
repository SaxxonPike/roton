namespace Lyon.App
{
    public interface IFileSystemFactory
    {
        IFileSystem Create(string path, string defaultWorld);
    }
}