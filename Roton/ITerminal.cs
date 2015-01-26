using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface ITerminal
    {
        void Plot(int x, int y, AnsiChar ac);
        void SetSize(int width, int height, bool wide);
        void Write(int x, int y, string value, int color);
    }
}
