using Roton.FileIo;

namespace Roton.Core
{
    public class EngineConfiguration : IEngineConfiguration
    {
        public IFileSystem Disk { get; set; }
        public bool EditorMode { get; set; }
        public IKeyboard Keyboard { get; set; }
        public ISpeaker Speaker { get; set; }
        public ITerminal Terminal { get; set; }
    }
}