using Roton.Core;

namespace Roton.Interface.Video.Scenes.Composition
{
    public interface ISceneComposer : ITerminal
    {
        int Rows { get; }
        void Update(int x, int y);
        int Columns { get; }
    }
}
