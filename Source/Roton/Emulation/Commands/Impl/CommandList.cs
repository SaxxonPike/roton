using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class CommandList : ICommandList
{
    private readonly Dictionary<string, ICommand> _commands;

    public CommandList(IContextMetadataService contextMetadataService,
        IEnumerable<ICommand> commands)
    {
        var result = new Dictionary<string, ICommand>();

        foreach (var command in commands)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(command))
                result.Add(attribute.Name, command);
        }

        _commands = result;
    }

    public ICommand Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in _commands)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}