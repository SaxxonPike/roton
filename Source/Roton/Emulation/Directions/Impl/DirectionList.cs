using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DirectionList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<IDirection>(contextMetadataService, serviceProvider), IDirectionList;