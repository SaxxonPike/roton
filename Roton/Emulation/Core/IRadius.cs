using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Execution;

namespace Roton.Core
{
    public interface IRadius
    {
        void Update(IXyPair location, RadiusMode mode);
    }
}