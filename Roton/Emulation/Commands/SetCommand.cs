using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "SET")]
    [ContextEngine(ContextEngine.Super, "SET")]
    public sealed class SetCommand : ICommand
    {
        private readonly IEngine _engine;

        public SetCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var flag = _engine.Parser.ReadWord(context.Index, context);
            _engine.World.Flags.Add(flag);
        }
    }
}