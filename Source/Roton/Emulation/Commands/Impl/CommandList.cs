using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class CommandList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeListByName<ICommand>(contextMetadataService, serviceProvider), ICommandList;