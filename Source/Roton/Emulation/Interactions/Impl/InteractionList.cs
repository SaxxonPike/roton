using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class InteractionList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeListById<IInteraction>(contextMetadataService, serviceProvider), IInteractionList;