using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "PLAY")]
    [ContextEngine(ContextEngine.Super, "PLAY")]
    public sealed class PlayCommand : ICommand
    {
        private readonly IEngine _engine;

        public PlayCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var notes = _engine.Parser.ReadLine(context.Index, context);
            var sound = _engine.EncodeMusic(notes);
            _engine.PlaySound(-1, sound);
            context.NextLine = false;
        }
    }
}