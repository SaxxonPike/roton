using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface ITile
    {
        int Color { get; set; }
        int Id { get; set; }
        ITile Clone();
    }
}
