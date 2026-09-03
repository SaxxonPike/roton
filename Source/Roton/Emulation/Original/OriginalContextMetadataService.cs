using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalContextMetadataService() : ContextMetadataService(Context.Original);