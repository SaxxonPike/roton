using System.Collections.Generic;
using Roton.Emulation.Directions;

namespace Roton.Emulation.SuperZZT
{
    public class ZztDirections : Data.Impl.Directions
    {
        public ZztDirections(IEnumerable<IDirection> commands) : base(commands, new string[]{})
        {
        }
    }
}