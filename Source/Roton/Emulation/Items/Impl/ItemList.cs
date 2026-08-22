using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ItemList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeListByName<IItem>(contextMetadataService, serviceProvider), IItemList;