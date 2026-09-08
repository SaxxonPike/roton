using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Editors.Impl;

[Context(Context.Editor)]
internal sealed class GameEditor(IWorld world)
    : IGameEditor, IBoardAccessor
{
    public IWorldEditor World { get; }

    public void Create(Context context)
    {
        throw new System.NotImplementedException();
    }

    public void EnsureBoard(int index)
    {
        throw new System.NotImplementedException();
    }
}