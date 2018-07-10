using System.Collections.Generic;

namespace Roton.Emulation.Interactions
{
    public abstract class InteractionList : IInteractionList
    {
        private readonly IDictionary<int, IInteraction> _interactions;
        private readonly IInteraction _defaultInteraction;

        protected InteractionList(IDictionary<int, IInteraction> interactions, IInteraction defaultInteraction)
        {
            _interactions = interactions;
            _defaultInteraction = defaultInteraction;
        }
        
        public IInteraction Get(int index)
        {
            return _interactions.ContainsKey(index) ? _interactions[index] : _defaultInteraction;
        }
    }

}