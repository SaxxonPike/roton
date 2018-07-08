using Roton.Emulation.Data;

namespace Roton.Emulation.Commands
{
    public class ThenCommand : ICommand
    {
        public string Name => "THEN";
        
        public void Execute(IOopContext context)
        {
            // The actual code doesn't work this way.
            // We cheat a little by not advancing the execution counter.
            context.Resume = true;
            context.CommandsExecuted--;
        }
    }
}