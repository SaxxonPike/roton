using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface ITerminal
    {
        // WinForm properties included to allow ITerminal implementations to be
        // created and used interchangably with a single ITerminal declaration.
        int Top { get; set; }
        int Left { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        bool AutoSize { get; set; }

        IKeyboard Keyboard { get; }

        void Clear();
        void Plot(int x, int y, AnsiChar ac);
        void SetScale(int xScale, int yScale);
        void SetSize(int width, int height, bool wide);
        void Write(int x, int y, string value, int color);
    }
}
