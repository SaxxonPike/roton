using System;
using System.Linq;
#if NET10_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
#endif
using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Composers.Video.Scenes.Impl;

/// <inheritdoc />
internal sealed class SceneComposer : ISceneComposer
{
    private readonly IGlyphComposerFactory _glyphComposerFactory;
    private readonly IPaletteComposerFactory _paletteComposerFactory;
    private IGlyphComposer? _glyphComposer;
    private IPaletteComposer? _paletteComposer;

    /// <inheritdoc />
    public event EventHandler<ResizedEventData>? Resized;

    /// <summary>
    /// Character that is used when clearing the character buffer.
    /// </summary>
    private readonly AnsiChar _blankCharacter;

    /// <summary>
    /// Composed palette in BGRA format.
    /// </summary>
    private ReadOnlyMemory<int> _colors;

    /// <summary>
    /// Raw font data.
    /// </summary>
    private ReadOnlyMemory<byte> _fontData;

    /// <summary>
    /// A lookup table used to determine where to start setting pixels in the pixel buffer.
    /// </summary>
    private ReadOnlyMemory<int> _offsetLookUpTable;

    /// <summary>
    /// Raw palette data.
    /// </summary>
    private ReadOnlyMemory<byte> _paletteData;

    /// <summary>
    /// Bitmap that contains the pixels for the "blink on" state.
    /// </summary>
    private Bitmap? _blinkOnBitmap;

    /// <summary>
    /// Bitmap that contains the pixels for the "blink off" state.
    /// </summary>
    private Bitmap? _blinkOffBitmap;

    /// <summary>
    /// Number of array items in a row of the pixel buffer.
    /// </summary>
    private int _stride;

    /// <summary>
    /// If true, instead of blinking, characters with the highest color bit set
    /// will show with high-intensity color.
    /// </summary>
    private bool _useFullBrightBackgrounds;

    /// <summary>
    /// Blink state when the last call to <see cref="GetBitmap"/> was made.
    /// </summary>
    private bool _lastFetchBlinkStatus;

    /// <summary>
    /// Contains indices of characters that have been updated since the last call to <see cref="GetBitmap"/>.
    /// </summary>
    private Memory<bool> _dirtyIndices;

    /// <summary>
    /// Character buffer.
    /// </summary>
    private Memory<AnsiChar> _chars;

    public SceneComposer(
        IPaletteComposerFactory paletteComposerFactory,
        IGlyphComposerFactory glyphComposerFactory
    )
    {
        _blankCharacter = new AnsiChar();
        _paletteComposerFactory = paletteComposerFactory;
        _glyphComposerFactory = glyphComposerFactory;

        InitializePalette();
        InitializeFont();
        InitializeBitmaps();
    }

    /// <summary>
    /// Returns true if blinking characters should be shown. Changes
    /// state each 256ms depending on the current system tick count.
    /// </summary>
    private static bool BlinkIsOn =>
        ((Environment.TickCount >> 8) & 1) == 0;

    /// <inheritdoc />
    public Bitmap? GetBitmap(bool onlyIfUpdated = false)
    {
        var dirty = _dirtyIndices.Span;
        var updated = false;

        for (var i = 0; i < _dirtyIndices.Length; i++)
        {
            if (!dirty[i])
                continue;

            dirty[i] = false;
            DoUpdate(i);
            updated = true;
        }

        var blinkOn = BlinkIsOn;

        if (onlyIfUpdated && !updated && blinkOn == _lastFetchBlinkStatus)
            return null;

        _lastFetchBlinkStatus = blinkOn;

        return blinkOn
            ? _blinkOnBitmap
            : _blinkOffBitmap;
    }

    /// <inheritdoc />
    public bool UseFullBrightBackgrounds
    {
        get => _useFullBrightBackgrounds;
        set
        {
            _useFullBrightBackgrounds = value;
            UpdateAllBlinkingCharacters();
        }
    }

    /// <inheritdoc />
    public bool Wide { get; private set; }

    /// <inheritdoc />
    public int Columns { get; private set; }

    /// <inheritdoc />
    public int Rows { get; private set; }

    /// <inheritdoc />
    public void Clear()
    {
        for (var y = 0; y < Rows; y++)
        for (var x = 0; x < Columns; x++)
            Plot(x, y, _blankCharacter);
    }

    /// <inheritdoc />
    public void Plot(int x, int y, AnsiChar ac)
    {
        if (IsOutOfBounds(x, y))
            return;

        var index = GetBufferOffset(x, y);

        var existingAc = _chars.Span[index];
        if (existingAc == ac)
            return;

        _chars.Span[index] = ac;
        _dirtyIndices.Span[index] = true;
    }

    /// <inheritdoc />
    public AnsiChar Read(int x, int y)
    {
        return IsOutOfBounds(x, y)
            ? _blankCharacter
            : _chars.Span[GetBufferOffset(x, y)];
    }

    /// <inheritdoc />
    public void SetFont(byte[] data)
    {
        _fontData = data.ToArray();
        InitializeFont();
        InvalidateAll();
    }

    /// <inheritdoc />
    public void SetPalette(byte[] data)
    {
        _paletteData = data.ToArray();
        InitializePalette();
        InvalidateAll();
    }

    /// <inheritdoc />
    public void SetSize(int width, int height, bool wide)
    {
        Rows = height;
        Columns = width;
        Wide = wide;

        var charTotal = Columns * Rows;
        _chars = new AnsiChar[charTotal];

        InitializeFont();
        InitializeBitmaps();
        InvalidateAll();

        Resized?.Invoke(this, new ResizedEventData(width, height, wide));
    }

    /// <inheritdoc />
    public void Write(int x, int y, ReadOnlySpan<char> value, int color)
    {
        foreach (var b in value)
        {
            if (y >= Rows)
                break;

            while (x >= Columns)
            {
                x -= Columns;
                y++;
            }

            Plot(x++, y, new AnsiChar(Cp437.CharToByte(b), color));
        }
    }

    /// <summary>
    /// Draws a glyph to the scene bitmap.
    /// </summary>
    /// <param name="ac">
    /// Character to draw.
    /// </param>
    /// <param name="offset">
    /// Memory offset into the bitmap data. This should reflect the upper-left pixel.
    /// </param>
    /// <remarks>
    /// A SIMD implementation is provided for later .NET versions.
    /// </remarks>
    private void DrawGlyph(AnsiChar ac, int offset)
    {
        if (_glyphComposer?.ComposeGlyph(ac.Char) is not { } glyph)
            return;
        if (_blinkOnBitmap == null || _blinkOnBitmap.Bits.IsEmpty)
            return;

        var blinkBit = !_useFullBrightBackgrounds && (ac.Color & 0x80) != 0;
        var outputOnBits = _blinkOnBitmap.Bits;
        var outputOffBits = _blinkOffBitmap!.Bits;
        var colors = _colors.Span;
        var inputBits = glyph.Data.Span;
        var width = glyph.Width;
        var height = glyph.Height;
        var baseOffset = offset;
        var inputOffset = 0;
        var backgroundColor = _useFullBrightBackgrounds
            ? colors[ac.Color >> 4]
            : colors[(ac.Color >> 4) & 0x7];
        var foregroundColor = colors[ac.Color & 0x0F];

#if NET10_0_OR_GREATER
        var vFgOn = Vector128.Create(foregroundColor);
        var vFgOff = blinkBit ? Vector128.Create(backgroundColor) : Vector128.Create(foregroundColor);
        var vBg = Vector128.Create(backgroundColor);

        for (var y = 0; y < height; y++)
        {
            var x = 0;
            while (x <= width - 4)
            {
                var vIn = Vector128.LoadUnsafe(ref Unsafe.Add(ref MemoryMarshal.GetReference(inputBits), inputOffset));
                Vector128.ConditionalSelect(vIn, vFgOn, vBg).StoreUnsafe(ref outputOnBits[baseOffset + x]);
                Vector128.ConditionalSelect(vIn, vFgOff, vBg).StoreUnsafe(ref outputOffBits[baseOffset + x]);
                inputOffset += 4;
                x += 4;
            }

            baseOffset += _stride;
        }
#else
        var foregroundColorOff = blinkBit ? backgroundColor : foregroundColor;

        for (var y = 0; y < height; y++)
        {
            var outputOffset = baseOffset;
            for (var x = 0; x < width; x++)
            {
                var inputBitData = inputBits[inputOffset++];
                outputOnBits[outputOffset++] = (inputBitData & foregroundColor) | (~inputBitData & backgroundColor);
                outputOffBits[outputOffset++] = (inputBitData & foregroundColorOff) | (~inputBitData & backgroundColor);
            }

            baseOffset += _stride;
        }
#endif
    }

    /// <summary>
    /// Gets the offset into the character array based on X/Y coordinates.
    /// </summary>
    private int GetBufferOffset(int x, int y) =>
        x + y * Columns;

    /// <summary>
    /// Creates new scene bitmaps.
    /// </summary>
    private void InitializeBitmaps()
    {
        if (_glyphComposer == null)
            return;

        var charTotal = Columns * Rows;
        var stride = Columns * _glyphComposer.MaxWidth;
        var height = Rows * _glyphComposer.MaxHeight;

        _offsetLookUpTable = Enumerable.Range(0, charTotal)
            .Select(i =>
                _glyphComposer.MaxWidth * (i % Columns) + _glyphComposer.MaxHeight * stride * (i / Columns))
            .ToArray();

        _dirtyIndices = new bool[charTotal];

        // Don't create new bitmaps if they are already the correct size.
        if (_blinkOnBitmap != null && _blinkOnBitmap.Height == height && _blinkOnBitmap.Width == stride)
            return;

        _stride = stride;
        _blinkOnBitmap = new Bitmap(stride, height);
        _blinkOffBitmap = new Bitmap(stride, height);
    }

    /// <summary>
    /// Composes glyphs for the current font.
    /// </summary>
    private void InitializeFont()
    {
        var oldGlyphComposer = _glyphComposer;
        _glyphComposer = _glyphComposerFactory.Get(_fontData, Wide);

        if (oldGlyphComposer != null)
        {
            if (_glyphComposer.MaxHeight != oldGlyphComposer.MaxHeight ||
                _glyphComposer.MaxWidth != oldGlyphComposer.MaxWidth)
                InitializeBitmaps();
        }
    }

    /// <summary>
    /// Composes the current palette.
    /// </summary>
    private void InitializePalette()
    {
        _paletteComposer = _paletteComposerFactory.Get(_paletteData);

        _colors = Enumerable
            .Range(0, 16)
            .Select(i => _paletteComposer.ComposeColor(i).ToArgb())
            .ToArray();
    }

    /// <summary>
    /// Returns true if the X/Y coordinate is out of bounds of the character grid.
    /// </summary>
    private bool IsOutOfBounds(int x, int y) =>
        x < 0 || x >= Columns || y < 0 || y >= Rows;

    private void DoUpdate(int index)
    {
        DrawGlyph(_chars.Span[index], _offsetLookUpTable.Span[index]);
    }

    /// <summary>
    /// Invalidates all character indices, forcing a re-draw of the entire grid.
    /// </summary>
    private void InvalidateAll() =>
        _dirtyIndices.Span.Fill(true);

    /// <summary>
    /// Forces an update of each character with the highest color bit set.
    /// </summary>
    private void UpdateAllBlinkingCharacters()
    {
        var dirty = _dirtyIndices.Span;

        for (var y = 0; y < Rows; y++)
        for (var x = 0; x < Columns; x++)
        {
            var index = GetBufferOffset(x, y);
            var c = _chars.Span[index];

            if ((c.Color & 0x80) != 0)
                dirty[index] = true;
        }
    }
}