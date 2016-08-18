using System;

namespace Roton.Interface.Video.Scenes.Composition
{
    public interface IBitmapSceneComposer : ISceneComposer, IDisposable
    {
        IFastBitmap Bitmap { get; }
    }
}