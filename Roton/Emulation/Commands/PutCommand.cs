using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class PutCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IPlotter _plotter;
        private readonly IMessager _messager;
        private readonly IConfig _config;
        private readonly ITiles _tiles;

        public PutCommand(IParser parser, IPlotter plotter, IMessager messager, IConfig config, ITiles tiles)
        {
            _parser = parser;
            _plotter = plotter;
            _messager = messager;
            _config = config;
            _tiles = tiles;
        }
        
        public string Name => "PUT";
        
        public void Execute(IOopContext context)
        {
            var vector = _parser.GetDirection(context);
            var success = false;

            if (vector != null)
            {
                var kind = _parser.GetKind(context);
                if (kind != null)
                {
                    success = true;
                    
                    var target = context.Actor.Location.Sum(vector);
                    if (!_config.BuggyPut || target.Y < _tiles.Height)
                        _plotter.Put(target, vector, kind);
                }
            }

            if (!success)
                _messager.RaiseError("Bad #PUT");
        }
    }
}