using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class ClearCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IFlags _flags;

        public ClearCommand(IParser parser, IFlags flags)
        {
            _parser = parser;
            _flags = flags;
        }
        
        public string Name => "CLEAR";
        
        public void Execute(IOopContext context)
        {
            var flag = _parser.ReadWord(context.Index, context);
            _flags.Remove(flag);
        }
    }
}