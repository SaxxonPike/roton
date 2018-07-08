using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core
{
    public interface IRadius
    {
        void Update(IXyPair location, RadiusMode mode);
    }
}