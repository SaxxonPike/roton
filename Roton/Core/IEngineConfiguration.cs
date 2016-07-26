using Roton.FileIo;

namespace Roton.Core
{
    public interface IEngineConfiguration
    {
        IFileSystem Disk { get; }
        bool EditorMode { get; }
        IKeyboard Keyboard { get; }
        int RandomSeed { get; }
        ISpeaker Speaker { get; }
        ITerminal Terminal { get; }
    }
}