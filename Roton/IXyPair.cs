using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface IXyPair
    {
        int X { get; set; }
        int Y { get; set; }
        void CopyFrom(IXyPair other);
        IXyPair Clone();
    }
}
