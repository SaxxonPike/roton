using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SeekDirection : IDirection
    {
        private readonly IEngine _engine;

        public SeekDirection(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "SEEK";
        
        public IXyPair Execute(IOopContext context)
        {
            return _engine.Seek(context.Actor.Location);
        }
    }
}