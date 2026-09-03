using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ConditionList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<ICondition>(contextMetadataService, serviceProvider), IConditionList;