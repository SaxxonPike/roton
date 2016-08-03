using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Roton.Core;
using Roton.Interface.Input;
using Roton.Interface.Video.Renderer;
using Message = System.Windows.Forms.Message;

namespace Roton.Interface.Video
{
    public partial class Terminal : UserControl, IEditorTerminal
    {
        private static readonly Encoding DosEncoding = Encoding.GetEncoding(437);
        private readonly KeysBuffer _keys;

        private IFastBitmap _bitmap;
        private readonly IRenderer _renderer;
        private bool _shiftHoldX;
        private bool _shiftHoldY;
        private AnsiChar[] _terminalBuffer;
        private IRasterFont _terminalFont;
        private int _terminalHeight;
        private IPalette _terminalPalette;
        private int _terminalWidth;
        private bool _wideMode;

        public Terminal(IRenderer renderer)
        {
            _keys = new KeysBuffer();

            InitializeComponent();

            // Initialize font and palette.
            _terminalFont = new RasterFont();
            _terminalPalette = new Palette();

            // Set default scale.
            ScaleX = 1;
            ScaleY = 1;

            // Set renderer.
            _renderer = renderer;
        }

        public bool Alt
        {
            get { return _keys.Alt; }
            set { _keys.Alt = value; }
        }

        private IFastBitmap Bitmap {
            get { return _bitmap; }
            set { _bitmap = value; }
        }

        // Auto-properties.
        public bool BlinkEnabled { get; set; }
        private bool Blinking { get; set; }

        public bool Control
        {
            get { return _keys.Control; }
            set { _keys.Control = value; }
        }

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
            return Bitmap.CloneAsBitmap();
        }

        public Bitmap RenderSingle(int character, int color)
        {
            color = TranslateColorIndex(color);
            var result = _terminalFont.RenderUnscaled(character, _terminalPalette[color & 0xF],
                _terminalPalette[(color >> 4) & 0xF]);
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
                Width = _terminalWidth*_terminalFont.Width*xScale*(_wideMode ? 2 : 1);
                Height = _terminalHeight*_terminalFont.Height*yScale;
            }

            _renderer.UpdateViewport();
        }

        public IRasterFont TerminalFont
        {
            get { return _terminalFont; }
            set
            {
                _terminalFont = value;
                Redraw();
            }
        }

        public IPalette TerminalPalette
        {
            get { return _terminalPalette; }
            set
            {
                _terminalPalette = value;
                Redraw();
            }
        }

        public void Clear()
        {
            _terminalBuffer = new AnsiChar[_terminalWidth*_terminalHeight];
        }

        public void Plot(int x, int y, AnsiChar ac)
        {
            if (x >= 0 && x < _terminalWidth && y >= 0 && y < _terminalHeight)
            {
                var index = x + y*_terminalWidth;
                _terminalBuffer[index] = ac;
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
            var oldBitmap = Bitmap;
            Bitmap = new FastBitmap(_terminalWidth*_terminalFont.Width, _terminalHeight*_terminalFont.Height);
            oldBitmap?.Dispose();

            if (width != oldWidth || height != oldHeight)
                Clear();

            if (AutoSize)
            {
                Width = _terminalWidth*_terminalFont.Width*ScaleX*(wide ? 2 : 1);
                Height = _terminalHeight*_terminalFont.Height*ScaleY;
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
            //SuspendLayout();
            Blinking = !Blinking;
            if (_terminalWidth > 0 && _terminalHeight > 0 && (Bitmap != null))
            {
                var i = 0;
                for (var y = 0; y < _terminalHeight; y++)
                {
                    for (var x = 0; x < _terminalWidth; x++)
                    {
                        if ((_terminalBuffer[i].Color & 0x80) != 0)
                        {
                            Draw(x, y, _terminalBuffer[i]);
                        }
                        i++;
                    }
                }
            }
            //ResumeLayout();
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            Redraw();
            _renderer.Render(ref _bitmap);
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
                var drawX = x*_terminalFont.Width;
                var drawY = y*_terminalFont.Height;
                var bgColor = _terminalPalette.Colors[(ac.Color >> 4) & (BlinkEnabled ? 0x7 : 0xF)];
                var fgColor = BlinkEnabled && Blinking && (ac.Color & 0x80) != 0
                    ? bgColor
                    : _terminalPalette.Colors[ac.Color & 0xF];
                _terminalFont.Render(Bitmap, drawX, drawY, ac.Char, fgColor, bgColor);
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
            Bitmap = new FastBitmap(glControl.Width, glControl.Height);

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
            Control = e.Control;
        }

        private void OnMouse(object sender, MouseEventArgs e)
        {
            if (CursorEnabled)
            {
                var newX = (int) (e.X/_terminalFont.Width*(_wideMode ? 0.5 : 1));
                var newY = e.Y/_terminalFont.Height;
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
            var index = 0;
            for (var y = 0; y < _terminalHeight; y++)
                for (var x = 0; x < _terminalWidth; x++)
                    Draw(x, y, _terminalBuffer[index++]);

            // Update the cursor if it's enabled and the bitmap is valid.
            if (!CursorEnabled || Bitmap == null) return;
            using (var g = Graphics.FromImage((FastBitmap) Bitmap))
            {
                using (
                    Pen bright = new Pen(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD)),
                        dark = new Pen(Color.FromArgb(0xFF, 0x22, 0x22, 0x22)))
                {
                    var outerRect = new Rectangle(CursorX*_terminalFont.Width, CursorY*_terminalFont.Height,
                        _terminalFont.Width - 1, _terminalFont.Height - 1);
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