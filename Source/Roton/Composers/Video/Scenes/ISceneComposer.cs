using System;
using JetBrains.Annotations;
using Roton.Composers.Video.Palettes;
using Roton.Emulation.Core;

namespace Roton.Composers.Video.Scenes;

/// <summary>
/// Handles rendering font and palette data into a linear bitmap.
/// </summary>
[PublicAPI]
public interface ISceneComposer : ITerminal
{
    /// <summary>
    /// Raised when the font data has changed.
    /// </summary>
    event EventHandler<FontDataChangedEventArgs>? FontDataChanged;
    
    /// <summary>
    /// Raised when the palette data has changed.
    /// </summary>
    event EventHandler<PaletteDataChangedEventArgs>? PaletteDataChanged;
    
    /// <summary>
    /// Raised when the pixel area has changed in size.
    /// </summary>
    event EventHandler<ResizedEventArgs>? Resized;
    
    /// <summary>
    /// Raised when the bitmap data has changed.
    /// </summary>
    event EventHandler<SceneUpdatedEventArgs>? SceneUpdated;

    /// <summary>
    /// Number of character rows.
    /// </summary>
    int Rows { get; }
    
    /// <summary>
    /// Number of character columns.
    /// </summary>
    int Columns { get; }
    
    /// <summary>
    /// Retrieves the bitmap data.
    /// </summary>
    /// <param name="onlyIfUpdated">
    /// If true, will only return the bitmap if it has changed since the last call.
    /// </param>
    Bitmap? GetBitmap(bool onlyIfUpdated);
    
    /// <summary>
    /// If true, instead of blinking characters, the full bright background color will be used.
    /// </summary>
    bool UseFullBrightBackgrounds { get; set; }
    
    /// <summary>
    /// If true, the scene will be rendered as double width.
    /// </summary>
    bool Wide { get; }
}