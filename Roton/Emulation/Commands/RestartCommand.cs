using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class RestartCommand : ICommand
    {
        public string Name => "RESTART";
        
        public void Execute(IOopContext context)
        {
            context.Instruction = 0;
            context.NextLine = false;
        }
    }
}