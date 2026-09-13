using System;
using JetBrains.Annotations;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Composers.Video.Scenes;

/// <summary>
/// Handles rendering font and palette data into a linear bitmap.
/// </summary>
[PublicAPI]
public interface ISceneComposer
{
    /// <summary>
    /// Raised when the pixel area has changed in size.
    /// </summary>
    event EventHandler<ResizedEventData>? Resized;

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
    
    void Clear();
    void Plot(int x, int y, AnsiChar ac);
    AnsiChar Read(int x, int y);
    void SetSize(int width, int height, bool wide);
    void Write(int x, int y, ReadOnlySpan<char> value, int color);
    void SetFont(byte[] data);
    void SetPalette(byte[] data);
}