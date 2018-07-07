using System.Collections.Generic;
using Roton.Emulation.Directions;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztDirections : Directions
    {
        public SuperZztDirections(IEnumerable<IDirection> commands) : base(commands, new string[]{})
        {
        }
    }
}