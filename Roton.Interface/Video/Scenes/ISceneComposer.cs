using Roton.Emulation.Core;
using Roton.Interface.Events;

namespace Roton.Interface.Video.Scenes
{
    public interface ISceneComposer : ITerminal
    {
        event SetSizeEventHandler AfterSetSize;
        int Rows { get; }
        void Update(int x, int y);
        int Columns { get; }
    }
}
