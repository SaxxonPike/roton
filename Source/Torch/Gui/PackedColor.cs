using System.Numerics;
using System.Runtime.Intrinsics;
using JetBrains.Annotations;

namespace Torch.Gui;

/// <summary>
/// Handles conversion of ImGui packed colors.
/// </summary>
[PublicAPI]
public static class PackedColor
{
    /// <summary>
    /// Packs a Float x4 color into <see cref="uint"/>.
    /// </summary>
    public static uint Encode(Vector128<float> col)
    {
        //
        // Probably fine to use Native functions here since NaN
        // really shouldn't be passed in as a color value.
        //

        return Vector128.Sum(
            Vector128.ConvertToUInt32Native(
                Vector128.ClampNative(
                    col,
                    Vector128<float>.Zero,
                    Vector128<float>.One
                ) * 255f
            ) * Vector128.Create(
                1U, 1U << 8, 1U << 16, 1U << 24
            )
        );
    }

    public static uint Encode(Vector4 col) =>
        Encode(col.AsVector128());

    public static uint Encode(SDL_FColor col) =>
        Encode(Vector128.LoadUnsafe(ref col.r));
}