using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class PlayCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly ISounder _sounder;

        public PlayCommand(IParser parser, ISounder sounder)
        {
            _parser = parser;
            _sounder = sounder;
        }
        
        public string Name => "PLAY";
        
        public void Execute(IOopContext context)
        {
            var notes = _parser.ReadLine(context.Index, context);
            var sound = _sounder.EncodeMusic(notes);
            _engine.PlaySound(-1, sound);
            context.NextLine = false;
        }
    }
}