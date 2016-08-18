using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Roton.Interface.Video.Scenes.Composition
{
    public sealed class DirectAccessBitmap : IDirectAccessBitmap
    {
        public Bitmap InnerBitmap { get; }
        public int[] Bits { get; }
        private bool Disposed { get; set; }
        public int Height { get; }
        public int Width { get; }
        private GCHandle BitsHandle { get; }
        public IntPtr BitsPointer { get; private set; }

        public DirectAccessBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new int[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            BitsPointer = BitsHandle.AddrOfPinnedObject();
            InnerBitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsPointer);
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            InnerBitmap.Dispose();
            BitsPointer = IntPtr.Zero;
            BitsHandle.Free();
        }
    }
}
