using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class BecomeCommand : ICommand
    {
        private readonly IEngine _engine;

        public BecomeCommand(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "BECOME";

        public void Execute(IOopContext context)
        {
            var kind = _engine.Parser.GetKind(context);
            if (kind == null)
            {
                _engine.RaiseError($"Bad #{Name}");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }
    }
}