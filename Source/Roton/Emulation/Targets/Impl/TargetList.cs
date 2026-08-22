using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class TargetList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeListByName<ITarget>(contextMetadataService, serviceProvider), ITargetList;