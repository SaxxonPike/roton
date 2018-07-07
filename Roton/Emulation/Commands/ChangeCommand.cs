using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class ChangeCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IElements _elements;
        private readonly IPlotter _plotter;
        private readonly IMessenger _messenger;
        private readonly ITiles _tiles;

        public ChangeCommand(IParser parser, IElements elements, IPlotter plotter, IMessenger messenger, ITiles tiles)
        {
            _parser = parser;
            _elements = elements;
            _plotter = plotter;
            _messenger = messenger;
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
                _messenger.RaiseError($"Bad #{Name}");
            }
        }
    }
}