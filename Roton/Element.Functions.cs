using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Element
    {
        internal Element()
        {
            this.Index = -1;
        }

        public override string ToString()
        {
            return KnownName ?? "";
        }
    }
}
