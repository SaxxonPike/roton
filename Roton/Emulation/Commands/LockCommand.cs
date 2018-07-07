using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class LockCommand : ICommand
    {
        public string Name => "LOCK";
        
        public void Execute(IOopContext context)
        {
            context.Actor.P2 = 1;
        }
    }
}