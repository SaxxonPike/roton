using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface IKeyboard
    {
        bool Alt { get; }
        void Clear();
        bool Control { get; }
        int GetKey();
        bool Shift { get; }
    }
}
