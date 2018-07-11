using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "LOCK")]
    [ContextEngine(ContextEngine.SuperZzt, "LOCK")]
    public sealed class LockCommand : ICommand
    {
        private readonly IEngine _engine;

        public LockCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            _engine.LockActor(context.Index);
        }
    }
}