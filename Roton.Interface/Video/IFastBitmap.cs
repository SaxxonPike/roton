using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting;

namespace Roton.Interface.Video
{
    public interface IFastBitmap : IDisposable
    {
        int[] Bits { get; }
        int Flags { get; }
        Guid[] FrameDimensionsList { get; }
        int Height { get; }
        float HorizontalResolution { get; }
        SizeF PhysicalDimension { get; }
        int PixelCount { get; }
        PixelFormat PixelFormat { get; }
        int[] PropertyIdList { get; }
        PropertyItem[] PropertyItems { get; }
        ImageFormat RawFormat { get; }
        Size Size { get; }
        float VerticalResolution { get; }
        int Width { get; }
        ColorPalette Palette { get; set; }
        object Tag { get; set; }

        IFastBitmap Clone();
        Bitmap Clone(RectangleF rect, PixelFormat format);
        Bitmap Clone(Rectangle rect, PixelFormat format);
        Bitmap CloneAsBitmap();
        ObjRef CreateObjRef(Type requestedType);
        RectangleF GetBounds(ref GraphicsUnit pageUnit);
        EncoderParameters GetEncoderParameterList(Guid encoder);
        int GetFrameCount(FrameDimension dimension);
        IntPtr GetHbitmap();
        IntPtr GetHbitmap(Color background);
        IntPtr GetHicon();
        Color GetPixel(int x, int y);
        PropertyItem GetPropertyItem(int propid);

        Image GetThumbnailImage(int thumbWidth, int thumbHeight, Image.GetThumbnailImageAbort callback,
            IntPtr callbackData);

        object InitializeLifetimeService();
        BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format);
        BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format, BitmapData bitmapData);
        void MakeTransparent();
        void MakeTransparent(Color transparentColor);
        void RemovePropertyItem(int propid);
        void RotateFlip(RotateFlipType rotateFlipType);
        void Save(string filename);
        void Save(string filename, ImageFormat format);
        void Save(Stream stream, ImageFormat format);
        void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams);
        void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams);
        void SaveAdd(EncoderParameters encoderParams);
        void SaveAdd(Image image, EncoderParameters encoderParams);
        int SelectActiveFrame(FrameDimension dimension, int frameIndex);
        void SetPixel(int x, int y, Color color);
        void SetPropertyItem(PropertyItem propitem);
        void SetResolution(float xDpi, float yDpi);
        void UnlockBits(BitmapData bitmapData);
    }
}