using System.IO;

namespace Lyon.App
{
    public interface IGame
    {
        void Run();
        void Run(Stream stream);
        void Run(string path);
    }
}