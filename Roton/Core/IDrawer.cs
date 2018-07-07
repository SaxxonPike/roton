namespace Roton.Core
{
    public interface IDrawer
    {
        AnsiChar Draw(IXyPair location);
        void UpdateBoard(IXyPair location);
    }
}