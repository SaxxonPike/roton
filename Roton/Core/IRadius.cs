using Roton.Emulation.Execution;

namespace Roton.Core
{
    public interface IRadius
    {
        void Update(IXyPair location, RadiusMode mode);
    }
}