using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class ChangeCommand : ICommand
    {
        private readonly IEngine _engine;

        public ChangeCommand(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "CHANGE";
        
        public void Execute(IOopContext context)
        {
            var success = false;
            var source = _engine.Parser.GetKind(context);
            if (source != null)
            {
                var target = _engine.Parser.GetKind(context);
                if (target != null)
                {
                    var targetElement = _engine.Elements[target.Id];
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
                _engine.RaiseError($"Bad #{Name}");
            }
        }
    }
}