using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztGrammar : Grammar
    {
        public SuperZztGrammar(IColorList colors, IElementList elements) : base(colors, elements)
        {
        }
    }
}
