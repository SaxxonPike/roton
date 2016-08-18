using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Roton.Core;
using Roton.Interface.Extensions;
using Roton.Interface.Input;
using Roton.Interface.Resources;
using Roton.Interface.Video.Controls;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;
using Roton.Interface.Video.Renderer;
using Roton.Interface.Video.Scenes.Composition;
using Message = System.Windows.Forms.Message;

namespace Roton.Interface.Video
{
    public partial class Terminal : UserControl, IEditorTerminal
    {
        private static readonly Encoding DosEncoding = Encoding.GetEncoding(437);
        private readonly KeysBuffer _keys;

        private IBitmapSceneComposer _sceneComposer;
        private readonly IRenderer _renderer;
        private bool _shiftHoldX;
        private bool _shiftHoldY;
        private IGlyphComposer _glyphComposer;
        private int _terminalHeight;
        private IPaletteComposer _paletteComposer;
        private int _terminalWidth;
        private bool _wideMode;

        public Terminal(IRenderer renderer)
        {
            _terminalWidth = 80;
            _terminalHeight = 25;
            _keys = new KeysBuffer();

            InitializeComponent();

            // Initialize font and palette.
            _glyphComposer = new AutoDetectBinaryGlyphComposer(CommonResourceZipFileSystem.Default.GetFont());
            _paletteComposer = new VgaPaletteComposer(CommonResourceZipFileSystem.Default.GetPalette());

            // Set default scale.
            ScaleX = 1;
            ScaleY = 1;

            // Set renderer.
            _renderer = renderer;
        }

        private bool Alt
        {
            get { return _keys.Alt; }
            set { _keys.Alt = value; }
        }

        // Auto-properties.
        private bool BlinkEnabled { get; set; }

        public int ScaleX { get; private set; }
        public int ScaleY { get; private set; }

        public bool Shift
        {
            get { return _keys.Shift; }
            set { _keys.Shift = value; }
        }

        public void AttachKeyHandler(Form form)
        {
            form.KeyDown += (sender, e) => { OnKey(e); };
            form.KeyUp += (sender, e) => { OnKey(e); };
        }

        // Editor-specific properties.
        public bool CursorEnabled { get; set; }
        public int CursorX { get; set; }
        public int CursorY { get; set; }

        public Bitmap RenderAll()
        {
            return _sceneComposer.Bitmap.CloneAsBitmap();
        }

        public Bitmap RenderSingle(int character, int color)
        {
            color = TranslateColorIndex(color);
            var result = _glyphComposer
                .ComposeGlyph(character)
                .RenderToFastBitmap(_paletteComposer.ComposeColor(color & 0xF), _paletteComposer.ComposeColor(color >> 4))
                .CloneAsBitmap();
            if (_wideMode)
            {
                var wideResult = new Bitmap(result.Width*2, result.Height, result.PixelFormat);
                using (var g = Graphics.FromImage(wideResult))
                {
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(result, 0, 0, wideResult.Width, wideResult.Height);
                }
                result.Dispose();
                return wideResult;
            }
            return result;
        }

        public IKeyboard Keyboard => _keys as IKeyboard;

        public void SetScale(int xScale, int yScale)
        {
            ScaleX = xScale;
            ScaleY = yScale;

            if (AutoSize)
            {
                Width = _terminalWidth*_glyphComposer.MaxWidth*xScale*(_wideMode ? 2 : 1);
                Height = _terminalHeight*_glyphComposer.MaxHeight * yScale;
            }

            _renderer.UpdateViewport();
        }

        public IGlyphComposer GlyphComposer
        {
            get { return _glyphComposer; }
            set
            {
                _glyphComposer = value;
                Redraw();
            }
        }

        public IPaletteComposer PaletteComposer
        {
            get { return _paletteComposer; }
            set
            {
                _paletteComposer = value;
                Redraw();
            }
        }

        public void Clear()
        {
            _sceneComposer.Clear();
        }

        public void Plot(int x, int y, AnsiChar ac)
        {
            if (x >= 0 && x < _terminalWidth && y >= 0 && y < _terminalHeight)
            {
                _sceneComposer.SetChar(x, y, ac);
                Draw(x, y, ac);
            }
        }

        public void SetSize(int width, int height, bool wide)
        {
            if (width == 0 || height == 0)
                return;

            var oldWidth = _terminalWidth;
            var oldHeight = _terminalHeight;
            _renderer.TerminalWidth = _terminalWidth = width;
            _renderer.TerminalHeight = _terminalHeight = height;
            _wideMode = wide;

            // Ignore wide mode with bitmaps; all scaling will be handled by the GPU.
            var oldComposer = _sceneComposer;
            _sceneComposer = new BitmapSceneComposer(_glyphComposer, _paletteComposer, _terminalWidth, _terminalHeight);
            oldComposer?.Dispose();

            if (width != oldWidth || height != oldHeight)
                Clear();

            if (AutoSize)
            {
                Width = _terminalWidth*_glyphComposer.MaxWidth * ScaleX*(wide ? 2 : 1);
                Height = _terminalHeight*_glyphComposer.MaxHeight * ScaleY;
            }

            // Reconfigure OpenGL viewport.
            _renderer.UpdateViewport();
        }

        public void Write(int x, int y, string value, int color)
        {
            var ac = new AnsiChar {Color = color};
            var characters = DosEncoding.GetBytes(value);
            var count = characters.Length;

            while (x < 0)
            {
                x += _terminalWidth;
                y--;
            }

            for (var index = 0; index < count; index++)
            {
                ac.Char = characters[index];
                Plot(x, y, ac);
                x++;
                if (x >= _terminalWidth)
                {
                    x -= _terminalWidth;
                    y++;
                }
            }
        }

        private void Blink()
        {
            _sceneComposer.HideBlinkingCharacters = !_sceneComposer.HideBlinkingCharacters;
            if (_terminalWidth > 0 && _terminalHeight > 0 && (_sceneComposer != null))
            {
                var i = 0;
                for (var y = 0; y < _terminalHeight; y++)
                {
                    for (var x = 0; x < _terminalWidth; x++)
                    {
                        var existingChar = _sceneComposer.GetChar(x, y);
                        if ((existingChar.Color & 0x80) != 0)
                        {
                            Draw(x, y, existingChar);
                        }
                        i++;
                    }
                }
            }
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            Redraw();
            _renderer.Render(_sceneComposer.Bitmap);
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Draw(int x, int y, AnsiChar ac)
        {
            if (x >= 0 && x < _terminalWidth && y >= 0 && y < _terminalHeight)
            {
                _sceneComposer.SetChar(x, y, ac);
            }
        }

        private void glControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
            _keys.Press(e.KeyChar);
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            // Set up key and mouse events.
            glControl.KeyPress += glControl_KeyPress;
            glControl.MouseMove += glControl_MouseMove;
            glControl.MouseDown += glControl_MouseDown;
            
            // Initialize the Bitmap to make sure that the renderer doesn't explode.
            _sceneComposer = new BitmapSceneComposer(_glyphComposer, _paletteComposer, _terminalWidth, _terminalHeight);

            // Initialize and configure the renderer.
            _renderer.FormControl = glControl;
            _renderer.TerminalWidth = _terminalWidth;
            _renderer.TerminalHeight = _terminalHeight;
            _renderer.Init();
            _renderer.UpdateViewport();

            // Enable blinking by default; set timer for it.
            BlinkEnabled = true;
            timerDaemon.Start(Blink, 1f/0.2f);

            // Start the rendering timer.
            displayTimer.Enabled = true;
        }

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouse(this, e);
            OnMouseDown(e);
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouse(this, e);
            OnMouseMove(e);
        }

        private void OnKey(KeyEventArgs e)
        {
            if (!e.Shift)
            {
                Shift = false;
                _shiftHoldX = false;
                _shiftHoldY = false;
            }
            else
            {
                Shift = true;
            }
            Alt = e.Alt;
        }

        private void OnMouse(object sender, MouseEventArgs e)
        {
            if (CursorEnabled)
            {
                var newX = e.X / ScaleX / _glyphComposer.MaxWidth;
                var newY = e.Y / ScaleY / _glyphComposer.MaxHeight;
                UpdateCursor(newX, newY);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var keyAlt = (keyData & Keys.Alt) != 0;
            var keyControl = (keyData & Keys.Control) != 0;
            var keyRaw = keyData & Keys.KeyCode;
            IsKeyLocked(Keys.NumLock);
            _keys.CapsLock = IsKeyLocked(Keys.CapsLock);

            var result = base.ProcessCmdKey(ref msg, keyData);
            if (!keyAlt && !keyControl && keyRaw != Keys.ShiftKey)
            {
                OnKey(new KeyEventArgs(keyData));
                return _keys.Press(keyData);
            }
            return result;
        }

        private void Redraw()
        {
            if (_sceneComposer == null)
                return;

            for (var y = 0; y < _terminalHeight; y++)
                for (var x = 0; x < _terminalWidth; x++)
                    Draw(x, y, _sceneComposer.GetChar(x, y));

            // Update the cursor if it's enabled and the bitmap is valid.
            if (!CursorEnabled || _sceneComposer == null) return;
            using (var g = Graphics.FromImage((FastBitmap) _sceneComposer.Bitmap))
            {
                using (
                    Pen bright = new Pen(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD)),
                        dark = new Pen(Color.FromArgb(0xFF, 0x22, 0x22, 0x22)))
                {
                    var outerRect = new Rectangle(CursorX*_glyphComposer.MaxWidth, CursorY*_glyphComposer.MaxHeight,
                        _glyphComposer.MaxWidth - 1, _glyphComposer.MaxHeight - 1);
                    g.DrawLines(dark, new[]
                    {
                        new Point(outerRect.Left, outerRect.Bottom),
                        new Point(outerRect.Right, outerRect.Bottom),
                        new Point(outerRect.Right, outerRect.Top)
                    });
                    g.DrawLines(bright, new[]
                    {
                        new Point(outerRect.Left, outerRect.Bottom),
                        new Point(outerRect.Left, outerRect.Top),
                        new Point(outerRect.Right, outerRect.Top)
                    });
                }
            }
        }

        internal int TranslateColorIndex(int color)
        {
            // if blinking is enabled, we only get the first 8 colors for background
            if (BlinkEnabled)
            {
                color &= 0x7F;
            }
            return color;
        }

        private void UpdateCursor(int newX, int newY)
        {
            if (Shift && !_shiftHoldX && !_shiftHoldY)
            {
                if (CursorX == newX && CursorY != newY)
                    _shiftHoldX = true;
                else if (CursorX != newX && CursorY == newY)
                    _shiftHoldY = true;
            }

            if (_shiftHoldX)
            {
                newX = CursorX;
            }

            if (_shiftHoldY)
            {
                newY = CursorY;
            }

            if (newX != CursorX || newY != CursorY)
            {
                CursorX = newX;
                CursorY = newY;
            }
        }
    }
}