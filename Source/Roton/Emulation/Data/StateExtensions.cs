namespace Roton.Emulation.Data;

public static class StateExtensions
{
    /// <summary>
    /// See <see cref="IState.GetOopWord"/>.
    /// </summary>
    public static string GetOopWord(this IState state)
    {
        var buffer = (stackalloc char[byte.MaxValue]);
        state.GetOopWord(buffer);
        return buffer.ToString();
    }
}