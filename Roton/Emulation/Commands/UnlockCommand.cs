using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "UNLOCK")]
    [ContextEngine(ContextEngine.SuperZzt, "UNLOCK")]
    public sealed class UnlockCommand : ICommand
    {
        private readonly IEngine _engine;

        public UnlockCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            _engine.UnlockActor(context.Index);
        }
    }
}