using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class InteractionList : IInteractionList
    {
        private readonly IDictionary<int, IInteraction> _interactions = new Dictionary<int, IInteraction>();

        public InteractionList(IContextMetadataService contextMetadataService, IEnumerable<IInteraction> interactions)
        {
            foreach (var interaction in interactions)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(interaction))
                    _interactions.Add(attribute.Id, interaction);
            }
        }
        
        public IInteraction Get(int index)
        {
            return _interactions.ContainsKey(index) ? _interactions[index] : _interactions[-1];
        }
    }

}