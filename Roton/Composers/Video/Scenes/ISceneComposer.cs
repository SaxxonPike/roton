using Roton.Emulation.Core;

namespace Roton.Composers.Video.Scenes
{
    public interface ISceneComposer : ITerminal
    {
        event SetSizeEventHandler AfterSetSize;
        int Rows { get; }
        void Update(int x, int y);
        int Columns { get; }
    }
}
