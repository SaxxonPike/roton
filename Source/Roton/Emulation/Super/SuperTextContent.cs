using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperTextContent : TextContent
{
    public SuperTextContent() 
        : base(new Memory(), 0)
    {
    }

    protected override int ItemLength => 61;
    protected override int Capacity => 1024;
}