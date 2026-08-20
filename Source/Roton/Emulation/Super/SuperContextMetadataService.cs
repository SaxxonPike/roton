using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperContextMetadataService() : ContextMetadataService(Context.Super);