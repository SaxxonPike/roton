using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ActionList : IActionList
{
    private readonly Dictionary<int, IAction> _actions;

    public ActionList(IContextMetadataService contextMetadataService,
        IEnumerable<IAction> actions)
    {
        var result = new Dictionary<int, IAction>();

        foreach (var action in actions)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(action))
                result.Add(attribute.Id, action);
        }

        _actions = result;
    }

    public IAction Get(int index) =>
        _actions.TryGetValue(index, out var action) ? action : _actions[-1];
}