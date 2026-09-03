using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperContextMetadataService() : ContextMetadataService(Context.Super);