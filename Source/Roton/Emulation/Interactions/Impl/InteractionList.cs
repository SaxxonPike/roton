using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class InteractionList : IInteractionList
    {
        private readonly Lazy<IDictionary<int, IInteraction>> _interactions;

        public InteractionList(IContextMetadataService contextMetadataService, IEnumerable<IInteraction> interactions)
        {
            _interactions = new Lazy<IDictionary<int, IInteraction>>(() =>
            {
                var result = new Dictionary<int, IInteraction>();
                foreach (var interaction in interactions)
                {
                    foreach (var attribute in contextMetadataService.GetMetadata(interaction))
                        result.Add(attribute.Id, interaction);
                }

                return result;
            });
        }

        public IInteraction Get(int index)
        {
            return _interactions.Value.ContainsKey(index)
                ? _interactions.Value[index]
                : _interactions.Value[-1];
        }
    }
}