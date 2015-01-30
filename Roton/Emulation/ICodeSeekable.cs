using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal interface ICodeSeekable
    {
        int Instruction { get; set; }
    }
}
