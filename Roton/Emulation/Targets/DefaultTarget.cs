using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class DefaultTarget : ITarget
    {
        private readonly IActors _actors;
        private readonly IParser _parser;

        public DefaultTarget(IActors actors, IParser parser)
        {
            _actors = actors;
            _parser = parser;
        }

        public bool Execute(ISearchContext context)
        {
            while (context.SearchIndex < _actors.Count)
            {
                if (_actors[context.SearchIndex].Pointer != 0)
                {
                    var instruction = new Executable();
                    var firstByte = _parser.ReadByte(context.SearchIndex, instruction);
                    if (firstByte == 0x40)
                    {
                        var name = _parser.ReadWord(context.SearchIndex, instruction);
                        if (name == context.SearchTarget)
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