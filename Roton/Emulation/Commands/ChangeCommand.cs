using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class ChangeCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IEngine _engine;
        private readonly IElements _elements;
        private readonly IPlotter _plotter;
        private readonly IMessager _messager;
        private readonly ITiles _tiles;

        public ChangeCommand(IParser parser, IEngine engine, IElements elements, IPlotter plotter, IMessager messager, ITiles tiles)
        {
            _parser = parser;
            _engine = engine;
            _elements = elements;
            _plotter = plotter;
            _messager = messager;
            _tiles = tiles;
        }

        public string Name => "CHANGE";
        
        public void Execute(IOopContext context)
        {
            var success = false;
            var source = _parser.GetKind(context);
            if (source != null)
            {
                var target = _parser.GetKind(context);
                if (target != null)
                {
                    var targetElement = _elements[target.Id];
                    success = true;
                    if (target.Color == 0 && targetElement.Color < 0xF0)
                    {
                        target.Color = targetElement.Color;
                    }
                    var location = new Location(0, 1);
                    while (_tiles.FindTile(source, location))
                    {
                        _plotter.PlotTile(location, target);
                    }
                }
            }

            if (!success)
            {
                _messager.RaiseError($"Bad #{Name}");
            }
        }
    }
}