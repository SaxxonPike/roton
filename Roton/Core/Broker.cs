using Roton.Emulation.Execution;

namespace Roton.Core
{
    public class Broker : IBroker
    {
        private readonly IParser _parser;
        private readonly IState _state;

        public Broker(IParser parser, IState state)
        {
            _parser = parser;
            _state = state;
        }
        
        public bool ExecuteTransaction(IOopContext context, bool take)
        {
            // Does the item exist?
            var item = _parser.GetItem(context);
            if (item == null)
                return false;

            // Do we have a valid amount?
            var amount = _parser.ReadNumber(context.Index, context);
            if (amount <= 0)
                return true;

            // Modify value if we are taking.
            if (take)
                _state.OopNumber = -_state.OopNumber;

            // Determine if the result will be in range.
            var pendingAmount = item.Value + _state.OopNumber;
            if ((pendingAmount & 0xFFFF) >= 0x8000)
                return true;

            // Successful transaction.
            item.Value = pendingAmount;
            return false;            
        }
    }
}