using System;
using System.Drawing;

namespace Roton.Interface.Video.Scenes.Composition
{
    /// <summary>
    /// A wrapper for the .NET Framework Bitmap object which allows for direct pixel access.
    /// </summary>
    public interface IDirectAccessBitmap : IDisposable
    {
        /// <summary>
        /// Raw bits.
        /// </summary>
        int[] Bits { get; }

        /// <summary>
        /// Height of the bitmap, in pixels.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Width of the bitmap, in pixels.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Inner .NET Bitmap object.
        /// </summary>
        Bitmap InnerBitmap { get; }

        /// <summary>
        /// Pointer to the raw bit data.
        /// </summary>
        IntPtr BitsPointer { get; }
    }
}