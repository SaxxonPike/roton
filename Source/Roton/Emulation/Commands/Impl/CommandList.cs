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
    private readonly Lazy<IDictionary<string, ICommand>> _commands;
    private IDictionary<string, ICommand> Commands => _commands.Value;

    public CommandList(Lazy<IContextMetadataService> contextMetadataService, 
        Lazy<IEnumerable<ICommand>> commands)
    {
        _commands = new Lazy<IDictionary<string, ICommand>>(() =>
        {
            var result = new Dictionary<string, ICommand>();

            foreach (var command in commands.Value)
            {
                foreach (var attribute in contextMetadataService.Value.GetMetadata(command))
                    result.Add(attribute.Name, command);
            }

            return result;
        });
    }

    public ICommand Get(ReadOnlySpan<char> name)
    {
        foreach (var entry in Commands)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
    }
}