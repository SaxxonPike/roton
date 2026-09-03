using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public static class StateExtensions
{
    public static Vector GetCardinalVector(this IState state, int index) =>
        new(state.Vector4[index], state.Vector4[index + 4]);

}