using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Commands
{
    public class BindCommand : ICommand
    {
        private readonly IEngine _engine;

        public BindCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "BIND";
        
        public void Execute(IOopContext context)
        {
            var search = new SearchContext();
            var target = _engine.Parser.ReadWord(context.Index, context);
            search.SearchTarget = target;
            if (_engine.Parser.GetTarget(search))
            {
                var targetActor = _engine.Actors[search.SearchIndex];
                context.Actor.Pointer = targetActor.Pointer;
                context.Actor.Length = targetActor.Length;
                context.Instruction = 0;
            }
        }
    }
}