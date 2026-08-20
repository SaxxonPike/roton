using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalContextMetadataService() : ContextMetadataService(Context.Original);