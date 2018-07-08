using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class GiveCommand : ICommand
    {
        private readonly IEngine _engine;

        public GiveCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "GIVE";
        
        public void Execute(IOopContext context)
        {
            context.Resume = _engine.ExecuteTransaction(context, false);
            _engine.Hud.UpdateStatus();
        }
    }
}