﻿using System;
using System.Runtime.InteropServices;

namespace Roton.Composers.Video.Scenes.Impl
{
    public sealed class Bitmap : IBitmap
    {
        public int[] Bits { get; }
        private bool Disposed { get; set; }
        public int Height { get; }
        public int Width { get; }
        public int Stride => Width * 4;
        private GCHandle BitsHandle { get; }
        public IntPtr BitsPointer { get; private set; }

        public Bitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new int[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            BitsPointer = BitsHandle.AddrOfPinnedObject();
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            BitsPointer = IntPtr.Zero;
            BitsHandle.Free();
        }
    }
}
