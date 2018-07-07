using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class EndCommand : ICommand
    {
        private readonly IState _state;

        public EndCommand(IState state)
        {
            _state = state;
        }
        
        public string Name => "END";
        
        public void Execute(IOopContext context)
        {
            _state.OopByte = 0;
            context.Instruction = -1;
        }
    }
}