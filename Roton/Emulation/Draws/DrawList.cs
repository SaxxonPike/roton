using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class DrawList : IDrawList
    {
        private readonly IDictionary<int, IDraw> _draws = new Dictionary<int, IDraw>();

        public DrawList(IContextMetadataService contextMetadataService, IEnumerable<IDraw> draws)
        {
            foreach (var draw in draws)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(draw))
                    _draws.Add(attribute.Id, draw);
            }
        }
        
        public IDraw Get(int index)
        {
            return _draws.ContainsKey(index) ? _draws[index] : _draws[-1];
        }
    }
}