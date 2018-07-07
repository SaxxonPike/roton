using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class BecomeCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IMessager _messager;

        public BecomeCommand(IParser parser, IMessager messager)
        {
            _parser = parser;
            _messager = messager;
        }

        public string Name => "BECOME";

        public void Execute(IOopContext context)
        {
            var kind = _parser.GetKind(context);
            if (kind == null)
            {
                _messager.RaiseError($"Bad #{Name}");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }
    }
}