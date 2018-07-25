using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class DirectionList : IDirectionList
    {
        private readonly IDictionary<string, IDirection> _directions = new Dictionary<string, IDirection>();

        public DirectionList(IContextMetadataService contextMetadataService, IEnumerable<IDirection> directions)
        {
            foreach (var direction in directions)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(direction))
                    _directions.Add(attribute.Name, direction);
            }
        }
        
        public IDirection Get(string name)
        {
            return _directions.ContainsKey(name)
                ? _directions[name]
                : null;
        }        
    }
}