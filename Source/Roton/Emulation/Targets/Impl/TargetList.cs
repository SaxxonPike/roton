using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TargetList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<ITarget>(contextMetadataService, serviceProvider), ITargetList;