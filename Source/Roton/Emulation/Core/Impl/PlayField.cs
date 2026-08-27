using Roton.Emulation.Data;

namespace Roton.Emulation.Core.Impl;

public abstract class PlayField : IPlayField
{
    public virtual void DrawTile(int x, int y, AnsiChar ac)
    {
    }
}