using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "IF")]
    [ContextEngine(ContextEngine.SuperZzt, "IF")]
    public sealed class IfCommand : ICommand
    {
        private readonly IEngine _engine;

        public IfCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var condition = _engine.Parser.GetCondition(context);
            
            if (condition.HasValue)
                context.Resume = condition.Value;
        }
    }
}