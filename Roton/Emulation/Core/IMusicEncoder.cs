using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IMusicEncoder
    {
        ISound Encode(string music);
    }
}