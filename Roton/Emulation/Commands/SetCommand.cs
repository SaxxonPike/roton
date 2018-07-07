using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class SetCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IFlags _flags;

        public SetCommand(IParser parser, IFlags flags)
        {
            _parser = parser;
            _flags = flags;
        }
        
        public string Name => "SET";
        
        public void Execute(IOopContext context)
        {
            var flag = _parser.ReadWord(context.Index, context);
            _flags.Add(flag);
        }
    }
}