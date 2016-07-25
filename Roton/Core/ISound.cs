using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Core
{
    public interface ISound
    {
        int this[int index] { get; }
        int Length { get; }
    }
}
