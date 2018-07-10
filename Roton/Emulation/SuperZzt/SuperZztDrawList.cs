using System.Collections.Generic;
using Roton.Emulation.Draws;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztDrawList : DrawList
    {
        public SuperZztDrawList(IDictionary<int, IDraw> draws, IDraw defaultDraw) : base(draws, defaultDraw)
        {
        }
    }
}