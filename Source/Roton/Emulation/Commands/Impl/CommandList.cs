using System;
using System.Collections.Generic;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class CommandList : ICommandList
{
#if NET10_0_OR_GREATER
    private readonly Dictionary<string, ICommand>.AlternateLookup<ReadOnlySpan<char>> _commands;
#else
    private readonly Dictionary<string, ICommand> _commands;
#endif

    public CommandList(IContextMetadataService contextMetadataService,
        IEnumerable<ICommand> commands)
    {
        var result = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);

        foreach (var command in commands)
        {
            foreach (var attribute in contextMetadataService.GetMetadata(command))
                result.Add(attribute.Name, command);
        }

#if NET10_0_OR_GREATER
        _commands = result.GetAlternateLookup<ReadOnlySpan<char>>();
#else
        _commands = result;
#endif
    }

    public ICommand? Get(ReadOnlySpan<char> name)
    {
#if NET10_0_OR_GREATER
        return _commands.TryGetValue(name, out var value) ? value : null;
#else
        foreach (var entry in _commands)
        {
            if (name.Equals(entry.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return entry.Value;
        }

        return null;
#endif
    }
}