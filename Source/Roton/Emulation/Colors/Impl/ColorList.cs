using System;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Colors.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ColorList(IContextMetadataService contextMetadataService, IServiceProvider serviceProvider)
    : TypeList<IColor>(contextMetadataService, serviceProvider), IColorList;