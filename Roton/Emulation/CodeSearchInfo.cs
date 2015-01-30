using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal class CodeSearchInfo
    {
        public CodeSearchInfo(string label)
        {
            this.Index = 0;
            this.Label = label;
            this.Offset = 0;
        }

        virtual public int Index
        {
            get;
            set;
        }

        virtual public string Label
        {
            get;
            set;
        }

        virtual public int Offset
        {
            get;
            set;
        }
    }
}
