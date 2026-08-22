using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ConditionList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeListByName<ICondition>(contextMetadataService, serviceProvider), IConditionList;