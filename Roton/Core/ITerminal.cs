namespace Roton.Core
{
    public interface ITerminal
    {
        IKeyboard Keyboard { get; }
        bool AutoSize { get; set; }
        int Height { get; set; }
        int Left { get; set; }
        // WinForm properties included to allow ITerminal implementations to be
        // created and used interchangably with a single ITerminal declaration.
        int Top { get; set; }
        bool Visible { get; set; }
        int Width { get; set; }

        void Clear();
        void Plot(int x, int y, AnsiChar ac);
        void SetScale(int xScale, int yScale);
        void SetSize(int width, int height, bool wide);
        void Write(int x, int y, string value, int color);
    }
}