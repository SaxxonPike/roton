namespace Roton.Emulation.Core
{
    public interface IFileDialog
    {
        string Open(string title, string extension);
    }
}