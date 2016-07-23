// FastBitmap
// created by saxxonpike@gmail.com 2014-10-17

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;

namespace Roton.Common
{
    /// <summary>
    /// A replacement for the .NET Bitmap class that allows fast direct bit access.
    /// </summary>
    public sealed class FastBitmap : MarshalByRefObject, IFastBitmap
    {
        /* 1) Implementation copied from http://msdn.microsoft.com/en-us/library/system.drawing.bitmap%28v=vs.110%29.aspx
              because we can't subclass Image for some reason... but we want a decent drop-in replacement.

           2) We use Sealed for speed; this class doesn't really NEED to be so if you need to subclass it,
              just remove the "sealed" keyword and you should be fine.
         
           3) While we wrap Bitmap, it is not 100% compatible if somehow the pixel dimensions change. You should
              render it out to another bitmap, do your modifications, then render back into FastBitmap first.
         
           4) We do not implement ISerializable because we do not have access to Bitmap's GetObjectData method.
              If you really need this future, simply (implicitly) convert to Bitmap.
         
           5) Pixel format is frozen to Format32bppPArgb. Maybe in the future we'll support indexed modes...
        */

        /// <summary>
        /// Retrieve the FastBitmap's internal Bitmap object.
        /// </summary>
        public static implicit operator Bitmap(FastBitmap fb)
        {
            return fb.Bitmap;
        }

        /// <summary>
        /// Retrieve the FastBitmap's internal Image object.
        /// </summary>
        public static implicit operator Image(FastBitmap fb)
        {
            return fb.Bitmap;
        }

        /// <summary>
        /// Create a clone of a FastBitmap.
        /// </summary>
        private FastBitmap(IFastBitmap source)
        {
            Initialize(source.Width, source.Height);
            Array.Copy(source.Bits, Bits, PixelCount);
        }

        /// <summary>
        /// Create a FastBitmap, duplicating pixel data from another image.
        /// </summary>
        public FastBitmap(Image original)
        {
            Initialize(original);
        }

        /// <summary>
        /// Create a FastBitmap using bitmap data from the specified data stream.
        /// </summary>
        public FastBitmap(Stream stream)
        {
            using (var temp = new Bitmap(stream))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap using bitmap data from a file.
        /// </summary>
        public FastBitmap(string filename)
        {
            using (var temp = new Bitmap(filename))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap using bitmap data from another image, scaling it to a new size.
        /// </summary>
        public FastBitmap(Image original, Size newSize)
        {
            using (var temp = new Bitmap(original, newSize))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap with the specified dimensions in pixels.
        /// </summary>
        public FastBitmap(int width, int height)
        {
            Initialize(width, height);
        }

        /// <summary>
        /// Create a FastBitmap using bitmap data from the specified data stream.
        /// </summary>
        public FastBitmap(Stream stream, bool useIcm)
        {
            using (var temp = new Bitmap(stream, useIcm))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap using bitmap data from a file.
        /// </summary>
        public FastBitmap(string filename, bool useIcm)
        {
            using (var temp = new Bitmap(filename, useIcm))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap using bitmap data from a resource.
        /// </summary>
        public FastBitmap(Type type, string resource)
        {
            using (var temp = new Bitmap(type, resource))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap from the specified raw data.
        /// </summary>
        private FastBitmap(int width, int height, int[] data)
        {
            Initialize(width, height);
            Array.Copy(data, Bits, PixelCount);
        }

        /// <summary>
        /// Create a FastBitmap using bitmap data from another image, scaling it to a new size.
        /// </summary>
        public FastBitmap(Image original, int width, int height)
        {
            using (var temp = new Bitmap(original, width, height))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap with the specified size and the resolution from the Graphics object.
        /// </summary>
        public FastBitmap(int width, int height, Graphics g)
        {
            using (var temp = new Bitmap(width, height, g))
            {
                Initialize(temp);
            }
        }

        /// <summary>
        /// Create a FastBitmap with the specified pixel format. (Not fully implemented, pixel format is always forced to Format32bppPArgb.)
        /// </summary>
        public FastBitmap(int width, int height, PixelFormat format)
        {
            if (format != PixelFormat.Format32bppPArgb)
                throw new NotImplementedException(
                    "FastBitmap does not yet support pixel formats other than Format32bppPArgb.");
            Initialize(width, height);
        }

        /// <summary>
        /// Create a FastBitmap with the specified size, pixel format, and pixel data. (Not fully implemented, pixel format is always forced to Format32bppPArgb and pixel data is ignored.)
        /// </summary>
        public FastBitmap(int width, int height, int stride, PixelFormat format, IntPtr scan0)
        {
            if (format != PixelFormat.Format32bppPArgb)
                throw new NotImplementedException(
                    "FastBitmap does not yet support pixel formats other than Format32bppPArgb.");
            Initialize(width, height);
        }

        /// <summary>
        /// Destructor for FastBitmap.
        /// </summary>
        ~FastBitmap()
        {
            if (!Disposed)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Internal Bitmap object.
        /// </summary>
        private Bitmap Bitmap { get; set; }

        /// <summary>
        /// Raw bitmap data.
        /// </summary>
        public int[] Bits { get; private set; }

        /// <summary>
        /// Creates an exact copy of this Image.
        /// </summary>
        public IFastBitmap Clone()
        {
            return new FastBitmap(this as IFastBitmap);
        }

        /// <summary>
        /// Creates a copy of the section of this Bitmap defined by Rectangle structure and with a specified PixelFormat enumeration.
        /// </summary>
        public Bitmap Clone(Rectangle rect, PixelFormat format)
        {
            return Bitmap.Clone(rect, format);
        }

        /// <summary>
        /// Creates a copy of the section of this Bitmap defined with a specified PixelFormat enumeration.
        /// </summary>
        public Bitmap Clone(RectangleF rect, PixelFormat format)
        {
            return Bitmap.Clone(rect, format);
        }

        /// <summary>
        /// Creates an object that contains all the relevant information required to generate a proxy used to communicate with a remote object.
        /// </summary>
        public override ObjRef CreateObjRef(Type requestedType)
        {
            return Bitmap.CreateObjRef(requestedType);
        }

        /// <summary>
        /// Releases all resources used by this Image.
        /// </summary>
        public void Dispose()
        {
            Disposed = true;
            if (Handle.IsAllocated)
            {
                Handle.Free();
            }
            if (Bitmap != null)
            {
                Bitmap.Dispose();
                Bitmap = null;
            }
            Bits = null;
        }

        /// <summary>
        /// If true, Dispose() was called already.
        /// </summary>
        private bool Disposed { get; set; }

        /// <summary>
        /// Gets attribute flags for the pixel data of this Image.
        /// </summary>
        public int Flags => Bitmap.Flags;

        /// <summary>
        /// Gets an array of GUIDs that represent the dimensions of frames within this Image.
        /// </summary>
        public Guid[] FrameDimensionsList => Bitmap.FrameDimensionsList;

        /// <summary>
        /// Gets the bounds of the image in the specified unit.
        /// </summary>
        public RectangleF GetBounds(ref GraphicsUnit pageUnit)
        {
            return Bitmap.GetBounds(ref pageUnit);
        }

        /// <summary>
        /// Returns information about the parameters supported by the specified image encoder. 
        /// </summary>
        public EncoderParameters GetEncoderParameterList(Guid encoder)
        {
            return Bitmap.GetEncoderParameterList(encoder);
        }

        /// <summary>
        /// Returns the number of frames of the specified dimension.
        /// </summary>
        public int GetFrameCount(FrameDimension dimension)
        {
            return Bitmap.GetFrameCount(dimension);
        }

        /// <summary>
        /// Creates a GDI bitmap object from this Bitmap.
        /// </summary>
        public IntPtr GetHbitmap()
        {
            return Bitmap.GetHbitmap();
        }

        /// <summary>
        /// Creates a GDI bitmap object from this Bitmap.
        /// </summary>
        public IntPtr GetHbitmap(Color background)
        {
            return Bitmap.GetHbitmap(background);
        }

        /// <summary>
        /// Returns the handle to an icon.
        /// </summary>
        public IntPtr GetHicon()
        {
            return Bitmap.GetHicon();
        }

        /// <summary>
        /// Gets the color of the specified pixel in this Bitmap.
        /// </summary>
        public Color GetPixel(int x, int y)
        {
            return Color.FromArgb(Bits[x + y*Height]);
        }

        /// <summary>
        /// Gets the specified property item from this Image.
        /// </summary>
        public PropertyItem GetPropertyItem(int propid)
        {
            return Bitmap.GetPropertyItem(propid);
        }

        /// <summary>
        /// Returns a thumbnail for this Image.
        /// </summary>
        public Image GetThumbnailImage(int thumbWidth, int thumbHeight, Image.GetThumbnailImageAbort callback,
            IntPtr callbackData)
        {
            return Bitmap.GetThumbnailImage(thumbWidth, thumbHeight, callback, callbackData);
        }

        /// <summary>
        /// Garbage collector handle for pinning purposes.
        /// </summary>
        private GCHandle Handle { get; set; }

        /// <summary>
        /// Gets the height, in pixels, of this Image.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the horizontal resolution, in pixels per inch, of this Image.
        /// </summary>
        public float HorizontalResolution => Bitmap.HorizontalResolution;

        /// <summary>
        /// Initialize the FastBitmap.
        /// </summary>
        private void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            PixelCount = width*height;
            Bits = new int[PixelCount];
            if (Handle.IsAllocated)
            {
                // this method should only ever be called once, but this exists to prevent memory leaks
                Handle.Free();
                Bitmap.Dispose();
                Bitmap = null;
            }
            Handle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width*4, PixelFormat.Format32bppPArgb, Handle.AddrOfPinnedObject());
        }

        /// <summary>
        /// Initialize a FastBitmap from another image.
        /// </summary>
        private void Initialize(Image image)
        {
            // convert to a bitmap so we can manipulate it
            var tempBitmap = new Bitmap(image);
            Initialize(tempBitmap.Width, tempBitmap.Height);

            // gain access to the image's raw bits
            var bits = tempBitmap.LockBits(new Rectangle(0, 0, tempBitmap.Width, tempBitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bits.Scan0, Bits, 0, PixelCount);
            tempBitmap.UnlockBits(bits);

            // clean up
            tempBitmap.Dispose();
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        public override object InitializeLifetimeService()
        {
            return Bitmap.InitializeLifetimeService();
        }

        /// <summary>
        /// Locks a Bitmap into system memory.
        /// </summary>
        public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format)
        {
            return Bitmap.LockBits(rect, flags, format);
        }

        /// <summary>
        /// Locks a Bitmap into system memory.
        /// </summary>
        public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format, BitmapData bitmapData)
        {
            return Bitmap.LockBits(rect, flags, format, bitmapData);
        }

        /// <summary>
        /// Makes the default transparent color transparent for this Bitmap.
        /// </summary>
        public void MakeTransparent()
        {
            Bitmap.MakeTransparent();
        }

        /// <summary>
        /// Makes the specified color transparent for this Bitmap.
        /// </summary>
        public void MakeTransparent(Color transparentColor)
        {
            Bitmap.MakeTransparent(transparentColor);
        }

        /// <summary>
        /// Gets or sets the color palette used for this Image.
        /// </summary>
        public ColorPalette Palette
        {
            get { return Bitmap.Palette; }
            set { Bitmap.Palette = value; }
        }

        /// <summary>
        /// Gets the width and height of this image.
        /// </summary>
        public SizeF PhysicalDimension => Bitmap.PhysicalDimension;

        /// <summary>
        /// Number of pixels in the bitmap.
        /// </summary>
        public int PixelCount { get; private set; }

        /// <summary>
        /// Gets the pixel format for this Image.
        /// </summary>
        public PixelFormat PixelFormat => PixelFormat.Format32bppPArgb;

        /// <summary>
        /// Gets IDs of the property items stored in this Image.
        /// </summary>
        public int[] PropertyIdList => Bitmap.PropertyIdList;

        /// <summary>
        /// Gets all the property items (pieces of metadata)
        /// </summary>
        public PropertyItem[] PropertyItems => Bitmap.PropertyItems;

        /// <summary>
        /// Gets the file format of this Image.
        /// </summary>
        public ImageFormat RawFormat => Bitmap.RawFormat;

        /// <summary>
        /// Removes the specified property item from this Image.
        /// </summary>
        public void RemovePropertyItem(int propid)
        {
            Bitmap.RemovePropertyItem(propid);
        }

        /// <summary>
        /// Rotates, flips, or rotates and flips the Image.
        /// </summary>
        public void RotateFlip(RotateFlipType rotateFlipType)
        {
            Bitmap.RotateFlip(rotateFlipType);
        }

        /// <summary>
        /// Saves this Image to the specified file or stream.
        /// </summary>
        public void Save(string filename)
        {
            Bitmap.Save(filename);
        }

        /// <summary>
        /// Saves this image to the specified stream in the specified format.
        /// </summary>
        public void Save(Stream stream, ImageFormat format)
        {
            Bitmap.Save(stream, format);
        }

        /// <summary>
        /// Saves this Image to the specified file in the specified format.
        /// </summary>
        public void Save(string filename, ImageFormat format)
        {
            Bitmap.Save(filename, format);
        }

        /// <summary>
        /// Saves this image to the specified stream, with the specified encoder and image encoder parameters.
        /// </summary>
        public void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            Bitmap.Save(stream, encoder, encoderParams);
        }

        /// <summary>
        /// Saves this Image to the specified file, with the specified encoder and image-encoder parameters.
        /// </summary>
        public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            Bitmap.Save(filename, encoder, encoderParams);
        }

        /// <summary>
        /// Adds a frame to the file or stream specified in a previous call to the Save method. Use this method to save selected frames from a multiple-frame image to another multiple-frame image.
        /// </summary>
        public void SaveAdd(EncoderParameters encoderParams)
        {
            Bitmap.SaveAdd(encoderParams);
        }

        /// <summary>
        /// Adds a frame to the file or stream specified in a previous call to the Save method.
        /// </summary>
        public void SaveAdd(Image image, EncoderParameters encoderParams)
        {
            Bitmap.SaveAdd(image, encoderParams);
        }

        /// <summary>
        /// Selects the frame specified by the dimension and index.
        /// </summary>
        public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
        {
            return Bitmap.SelectActiveFrame(dimension, frameIndex);
        }

        /// <summary>
        /// Sets the color of the specified pixel in this Bitmap.
        /// </summary>
        public void SetPixel(int x, int y, Color color)
        {
            Bits[x + y*Width] = color.ToArgb();
        }

        /// <summary>
        /// Stores a property item (piece of metadata) in this Image.
        /// </summary>
        public void SetPropertyItem(PropertyItem propitem)
        {
            Bitmap.SetPropertyItem(propitem);
        }

        /// <summary>
        /// Sets the resolution for this Bitmap.
        /// </summary>
        public void SetResolution(float xDpi, float yDpi)
        {
            Bitmap.SetResolution(xDpi, yDpi);
        }

        /// <summary>
        /// Gets the width and height, in pixels, of this image.
        /// </summary>
        public Size Size => new Size(Width, Height);

        /// <summary>
        /// Gets or sets an object that provides additional data about the image.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Unlocks this Bitmap from system memory.
        /// </summary>
        public void UnlockBits(BitmapData bitmapData)
        {
            Bitmap.UnlockBits(bitmapData);
        }

        /// <summary>
        /// Gets the vertical resolution, in pixels per inch, of this Image.
        /// </summary>
        public float VerticalResolution => Bitmap.VerticalResolution;

        /// <summary>
        /// Gets the width, in pixels, of this Image.
        /// </summary>
        public int Width { get; private set; }
    }
}