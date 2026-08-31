using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <inheritdoc cref="ICheatList" />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class CheatList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider)
    : TypeList<ICheat>(contextMetadataService, serviceProvider), ICheatList;