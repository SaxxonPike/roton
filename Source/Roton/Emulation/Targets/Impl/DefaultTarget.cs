using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class DefaultTarget : ITarget
    {
        private readonly Lazy<IActors> _actors;
        private IActors Actors => _actors.Value;
        
        private readonly Lazy<IParser> _parser;
        private IParser Parser => _parser.Value;

        public DefaultTarget(Lazy<IActors> actors, Lazy<IParser> parser)
        {
            _actors = actors;
            _parser = parser;
        }

        public bool Execute(int index, ISearchContext context, string term)
        {
            while (context.SearchIndex < Actors.Count)
            {
                if (Actors[context.SearchIndex].Pointer != 0)
                {
                    var instruction = new Executable();
                    var firstByte = Parser.ReadByte(context.SearchIndex, instruction);
                    if (firstByte == 0x40)
                    {
                        var name = Parser.ReadWord(context.SearchIndex, instruction);
                        if (name == term)
                        {
                            return true;
                        }
                    }
                }
                context.SearchIndex++;
            }
            return false;
        }
    }
}