using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ActionList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<IAction>(contextMetadataService, serviceProvider), IActionList;