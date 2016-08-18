using System;
using Roton.Core;

namespace Roton.Interface.Video.Scenes.Composition
{
    public interface IBitmapSceneComposer : ISceneComposer, IDisposable
    {
        IFastBitmap Bitmap { get; }
    }
}