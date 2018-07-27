using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [ContextEngine(ContextEngine.Original, "GO")]
    [ContextEngine(ContextEngine.Super, "GO")]
    public sealed class GoCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public GoCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var vector = Engine.Parser.GetDirection(context);
            if (vector != null)
            {
                var target = context.Actor.Location.Sum(vector);
                if (!Engine.Tiles.ElementAt(target).IsFloor)
                {
                    Engine.Push(target, vector);
                }
                if (Engine.Tiles.ElementAt(target).IsFloor)
                {
                    Engine.MoveActor(context.Index, target);
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