using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class UnlockCommand : ICommand
    {
        public string Name => "UNLOCK";
        
        public void Execute(IOopContext context)
        {
            context.Actor.P2 = 0;
        }
    }
}