using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.Commands
{
    public class BindCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IActors _actors;

        public BindCommand(IParser parser, IActors actors)
        {
            _parser = parser;
            _actors = actors;
        }
        
        public string Name => "BIND";
        
        public void Execute(IOopContext context)
        {
            var search = new SearchContext();
            var target = _parser.ReadWord(context.Index, context);
            search.SearchTarget = target;
            if (_parser.GetTarget(search))
            {
                var targetActor = _actors[search.SearchIndex];
                context.Actor.Pointer = targetActor.Pointer;
                context.Actor.Length = targetActor.Length;
                context.Instruction = 0;
            }
        }
    }
}