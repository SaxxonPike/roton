using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class GiveCommand : ICommand
    {
        private readonly IBroker _broker;
        private readonly IHud _hud;

        public GiveCommand(IBroker broker, IHud hud)
        {
            _broker = broker;
            _hud = hud;
        }
        
        public string Name => "GIVE";
        
        public void Execute(IOopContext context)
        {
            context.Resume = _broker.ExecuteTransaction(context, false);
            _hud.UpdateStatus();
        }
    }
}