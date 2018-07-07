using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class TakeCommand : ICommand
    {
        private readonly IBroker _broker;
        private readonly IHud _hud;

        public TakeCommand(IBroker broker, IHud hud)
        {
            _broker = broker;
            _hud = hud;
        }
        
        public string Name => "TAKE";
        
        public void Execute(IOopContext context)
        {
            context.Resume = _broker.ExecuteTransaction(context, true);
            _hud.UpdateStatus();
        }
    }
}