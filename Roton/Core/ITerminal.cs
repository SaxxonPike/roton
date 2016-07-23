namespace Roton.Core
{
    public interface ITerminal
    {
        void Clear();
        void Plot(int x, int y, AnsiChar ac);
        void SetSize(int width, int height, bool wide);
        void Write(int x, int y, string value, int color);
    }
}