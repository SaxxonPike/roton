using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class IdleCommand : ICommand
    {
        public string Name => "IDLE";
        
        public void Execute(IOopContext context)
        {
            context.Moved = true;
        }
    }
}