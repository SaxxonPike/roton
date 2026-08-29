using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ItemList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<IItem>(contextMetadataService, serviceProvider), IItemList;