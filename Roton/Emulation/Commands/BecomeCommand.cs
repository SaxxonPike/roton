using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class BecomeCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IMessenger _messenger;

        public BecomeCommand(IParser parser, IMessenger messenger)
        {
            _parser = parser;
            _messenger = messenger;
        }

        public string Name => "BECOME";

        public void Execute(IOopContext context)
        {
            var kind = _parser.GetKind(context);
            if (kind == null)
            {
                _messenger.RaiseError($"Bad #{Name}");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }
    }
}