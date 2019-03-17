using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "PLAY")]
    [Context(Context.Super, "PLAY")]
    public sealed class PlayCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public PlayCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var notes = Engine.Parser.ReadLine(context.Index, context);
            var sound = Engine.MusicEncoder.Encode(notes);
            Engine.PlaySound(-1, sound);
            context.NextLine = false;
        }
    }
}