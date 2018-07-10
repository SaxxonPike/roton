using System.Collections.Generic;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztInteractionList : InteractionList
    {
        public SuperZztInteractionList(IDictionary<int, IInteraction> interactions, IInteraction defaultInteraction) 
            : base(interactions, defaultInteraction)
        {
        }
    }
}