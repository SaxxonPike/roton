using System;

namespace Roton.Interface.Video.Scenes.Composition
{
    public interface IBitmapSceneComposer : ISceneComposer, IDisposable
    {
        IDirectAccessBitmap DirectAccessBitmap { get; }
        bool HideBlinkingCharacters { get; set; }
    }
}