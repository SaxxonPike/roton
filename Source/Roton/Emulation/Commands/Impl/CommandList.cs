using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class CommandList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<ICommand>(contextMetadataService, serviceProvider), ICommandList;