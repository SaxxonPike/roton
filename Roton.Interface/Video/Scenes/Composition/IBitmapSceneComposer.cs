using System;

namespace Roton.Interface.Video.Scenes.Composition
{
    /// <summary>
    /// A scene composer that renders to a bitmap.
    /// </summary>
    public interface IBitmapSceneComposer : ISceneComposer, IDisposable
    {
        IDirectAccessBitmap DirectAccessBitmap { get; }
        bool HideBlinkingCharacters { get; set; }
        bool UseFullBrightBackgrounds { get; set; }
    }
}