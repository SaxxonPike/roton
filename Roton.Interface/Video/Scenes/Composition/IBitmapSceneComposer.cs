using System;

namespace Roton.Interface.Video.Scenes.Composition
{
    /// <summary>
    /// A scene composer that renders to a bitmap.
    /// </summary>
    public interface IBitmapSceneComposer : ISceneComposer, IDisposable
    {
        /// <summary>
        /// The underlying bitmap.
        /// </summary>
        IDirectAccessBitmap DirectAccessBitmap { get; }

        /// <summary>
        /// If true, blinking characters will be replaced with a solid background color.
        /// </summary>
        bool HideBlinkingCharacters { get; set; }

        /// <summary>
        /// If true, blinking is disabled and the full range of 16 colors can be used in the background.
        /// </summary>
        bool UseFullBrightBackgrounds { get; set; }
    }
}