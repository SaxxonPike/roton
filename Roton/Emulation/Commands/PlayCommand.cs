using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Commands
{
    public class PlayCommand : ICommand
    {
        private readonly IEngine _engine;

        public PlayCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "PLAY";
        
        public void Execute(IOopContext context)
        {
            var notes = _engine.Parser.ReadLine(context.Index, context);
            var sound = _engine.EncodeMusic(notes);
            _engine.PlaySound(-1, sound);
            context.NextLine = false;
        }
    }
}