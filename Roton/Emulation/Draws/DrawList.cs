using System.Collections.Generic;

namespace Roton.Emulation.Draws
{
    public abstract class DrawList : IDrawList
    {
        private readonly IDictionary<int, IDraw> _draws;
        private readonly IDraw _defaultDraw;

        protected DrawList(IDictionary<int, IDraw> draws, IDraw defaultDraw)
        {
            _draws = draws;
            _defaultDraw = defaultDraw;
        }
        
        public IDraw Get(int index)
        {
            return _draws.ContainsKey(index) ? _draws[index] : _defaultDraw;
        }
    }
}