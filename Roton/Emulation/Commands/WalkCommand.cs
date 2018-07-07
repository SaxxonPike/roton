using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class WalkCommand : ICommand
    {
        private readonly IParser _parser;

        public WalkCommand(IParser parser)
        {
            _parser = parser;
        }

        public string Name => "WALK";
        
        public void Execute(IOopContext context)
        {
            var vector = _parser.GetDirection(context);
            if (vector != null)
            {
                context.Actor.Vector.CopyFrom(vector);
            }
        }
    }
}