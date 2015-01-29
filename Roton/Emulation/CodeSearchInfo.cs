using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class CodeSearchInfo
    {
        public int Index;
        public string Label;
        public int Offset;

        public CodeSearchInfo(string label)
        {
            this.Index = 0;
            this.Label = label;
            this.Offset = 0;
        }
    }
}
