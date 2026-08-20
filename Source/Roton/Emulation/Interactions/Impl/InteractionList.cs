using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class InteractionList : IInteractionList
{
    private readonly Dictionary<int, IInteraction> _interactions;

    public InteractionList(
        IContextMetadataService contextMetadataService,
        IEnumerable<IInteraction> interactions)
    {
        var result = new Dictionary<int, IInteraction>();
        foreach (var interaction in interactions)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(interaction))
                result.Add(attribute.Id, interaction);
        }

        _interactions = result;
    }

    public IInteraction Get(int index) =>
        _interactions.TryGetValue(index, out var value)
            ? value
            : _interactions[-1];
}