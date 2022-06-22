using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalTextContent : TextContent
{
    public OriginalTextContent() 
        : base(new Memory(), 0)
    {
    }

    protected override int ItemLength => 51;
    protected override int Capacity => 1024;
}