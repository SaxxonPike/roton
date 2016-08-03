using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal class Randomizer : IRandomizer
    {
        private readonly IRandomState _state;

        public Randomizer(IRandomState initialState)
        {
            _state = initialState;
        }

        public int GetNext(int exclusiveUpperBound)
        {
            unchecked
            {
                var newState = _state.State*33797 + 1;
                _state.State = newState;
            }

            if (exclusiveUpperBound == 0)
                return 0;

            return ((_state.State >> 16) & 0xFFFF) % exclusiveUpperBound;
        }
    }
}