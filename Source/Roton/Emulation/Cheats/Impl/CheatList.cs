using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class CheatList(
    IContextMetadataService contextMetadataService, 
    IServiceProvider serviceProvider)
    : TypeList<ICheat>(contextMetadataService, serviceProvider), ICheatList;
