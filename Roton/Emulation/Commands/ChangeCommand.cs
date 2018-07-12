using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "CHANGE")]
    [ContextEngine(ContextEngine.Super, "CHANGE")]
    public sealed class ChangeCommand : ICommand
    {
        private readonly IEngine _engine;

        public ChangeCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var success = false;
            var source = _engine.Parser.GetKind(context);
            if (source != null)
            {
                var target = _engine.Parser.GetKind(context);
                if (target != null)
                {
                    var targetElement = _engine.ElementList[target.Id];
                    success = true;
                    if (target.Color == 0 && targetElement.Color < 0xF0)
                    {
                        target.Color = targetElement.Color;
                    }
                    var location = new Location(0, 1);
                    while (_engine.Tiles.FindTile(source, location))
                    {
                        _engine.PlotTile(location, target);
                    }
                }
            }

            if (!success)
            {
                _engine.RaiseError($"Bad #CHANGE");
            }
        }
    }
}