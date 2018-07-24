using System;

namespace Roton.Composers.Video.Scenes
{
    /// <summary>
    /// A wrapper for the .NET Framework Bitmap object which allows for direct pixel access.
    /// </summary>
    public interface IBitmap : IDisposable
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
        /// Pointer to the raw bit data.
        /// </summary>
        IntPtr BitsPointer { get; }
        
        /// <summary>
        /// Stride of the bitmap.
        /// </summary>
        int Stride { get; }
    }
}