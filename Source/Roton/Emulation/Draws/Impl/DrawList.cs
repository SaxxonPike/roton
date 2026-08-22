using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DrawList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeListById<IDraw>(contextMetadataService, serviceProvider), IDrawList;