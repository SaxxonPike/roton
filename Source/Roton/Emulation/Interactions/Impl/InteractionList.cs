using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class InteractionList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<IInteraction>(contextMetadataService, serviceProvider), IInteractionList;