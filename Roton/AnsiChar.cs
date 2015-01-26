using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public struct AnsiChar
    {
        public int Char;
        public int Color;

        public AnsiChar(int newChar, int newColor)
        {
            this.Char = newChar;
            this.Color = newColor;
        }
    }
}
