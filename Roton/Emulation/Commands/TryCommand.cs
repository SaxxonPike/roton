using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "TRY")]
    [ContextEngine(ContextEngine.Super, "TRY")]
    public sealed class TryCommand : ICommand
    {
        private readonly IEngine _engine;

        public TryCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector == null)
                return;

            var target = vector.Sum(context.Actor.Location);
            if (!_engine.Tiles.ElementAt(target).IsFloor)
            {
                _engine.Push(target, vector);
            }
            if (_engine.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(context.Index, target);
                context.Moved = true;
                context.Resume = false;
            }
            else
            {
                context.Resume = true;
            }
        }
    }
}