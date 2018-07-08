using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class GoCommand : ICommand
    {
        private readonly IEngine _engine;

        public GoCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "GO";
        
        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector != null)
            {
                var target = context.Actor.Location.Sum(vector);
                if (!_engine.Tiles.ElementAt(target).IsFloor)
                {
                    _engine.Push(target, vector);
                }
                if (_engine.Tiles.ElementAt(target).IsFloor)
                {
                    _engine.MoveActor(context.Index, target);
                    context.Moved = true;
                }
                else
                {
                    context.Repeat = true;
                }
            }
        }
    }
}