using System.Collections.Generic;
using Roton.Emulation.Directions;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztDirections : Data.Impl.Directions
    {
        public SuperZztDirections(IEnumerable<IDirection> commands) : base(commands, new string[]{})
        {
        }
    }
}