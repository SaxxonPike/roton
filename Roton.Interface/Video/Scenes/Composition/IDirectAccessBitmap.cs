using System;
using System.Drawing;

namespace Roton.Interface.Video.Scenes.Composition
{
    public interface IDirectAccessBitmap : IDisposable
    {
        int[] Bits { get; }
        int Height { get; }
        int Width { get; }
        Bitmap InnerBitmap { get; }
        IntPtr BitsPointer { get; }
    }
}