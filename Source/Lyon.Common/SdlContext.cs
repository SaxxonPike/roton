using System.Collections.Concurrent;

namespace Lyon.Common;

/// <summary>
/// Manages reference counting for SDL subsystems.
/// </summary>
public sealed class SdlContext : IDisposable
{
    /// <summary>
    /// Holds the reference count for each SDL subsystem.
    /// </summary>
    private static readonly ConcurrentDictionary<SDL_InitFlags, int> RefCounts = [];

    /// <summary>
    /// If true, the context has been disposed and the reference counts have been decremented accordingly.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Flags that are initialized by this context.
    /// </summary>
    private SDL_InitFlags _flags;

    private SdlContext()
    {
    }

    /// <inheritdoc />
    ~SdlContext() => Dispose();

    /// <summary>
    /// Creates a new SdlContext.
    /// </summary>
    /// <param name="flags">
    /// Subsystems to ensure are initialized.
    /// </param>
    /// <param name="throwOnFailure">
    /// If true, failure will throw an exception.
    /// </param>
    /// <returns>
    /// The new context.
    /// </returns>
    /// <remarks>
    /// When finished with the SDL context, it should be disposed to free resources.
    /// </remarks>
    public static SdlContext Create(SDL_InitFlags flags, bool throwOnFailure = true)
    {
        var context = new SdlContext
        {
            _flags = flags
        };

        IncrementRefCount(flags, throwOnFailure);

        return context;
    }

    /// <summary>
    /// Increments the reference count for each bit in SDL_InitFlags by one.
    /// </summary>
    private static void IncrementRefCount(SDL_InitFlags flags, bool throwOnFailure)
    {
        foreach (var flag in SplitFlags(flags))
        {
            if (RefCounts.AddOrUpdate(flag, 1, (_, count) => count + 1) != 1)
                continue;

            var success = SDL_InitSubSystem(flag);
            if (throwOnFailure && !success)
                throw new SdlException();
        }
    }

    /// <summary>
    /// Decrements the reference count for each bit in SDL_InitFlags by one.
    /// </summary>
    private static void DecrementRefCount(SDL_InitFlags flags)
    {
        foreach (var flag in SplitFlags(flags))
        {
            if (RefCounts.AddOrUpdate(flag, 0, (_, count) => count - 1) == 0)
                SDL_QuitSubSystem(flag);
        }
    }

    /// <summary>
    /// Returns each bit in SDL_InitFlags.
    /// </summary>
    private static IEnumerable<SDL_InitFlags> SplitFlags(SDL_InitFlags flags)
    {
        var bits = (int)flags;

        while (bits != 0)
        {
            var flag = bits & -bits;
            yield return (SDL_InitFlags)flag;
            bits &= ~flag;
        }
    }

    /// <summary>
    /// Frees up initialized subsystems for this context.
    /// </summary>
    private void Destroy() =>
        DecrementRefCount(_flags);

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
            return;

        GC.SuppressFinalize(this);
        _isDisposed = true;
        Destroy();
    }
}