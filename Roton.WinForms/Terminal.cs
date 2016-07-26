using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Roton.Common;
using Roton.Core;
using Message = System.Windows.Forms.Message;

namespace Roton.WinForms
{
    public partial class Terminal : UserControl, IEditorTerminal
    {
        private static readonly Encoding CodePage437 = Encoding.GetEncoding(437);

        private int _cursorX;
        private int _cursorY;
        private KeysBuffer _keys;
        private bool _shiftHoldX;
        private bool _shiftHoldY;
        private AnsiChar[] _terminalBuffer;
        private IRasterFont _terminalFont;
        private int _terminalHeight;
        private IPalette _terminalPalette;
        private bool _terminalWide;
        private int _terminalWidth;
        private bool _updated;

        public Terminal()
        {
            // Initialize a default font/palette in case one isn't defined.
            _terminalFont = new RasterFont();
            _terminalPalette = new Palette();
            Initialize();
        }

        private bool Alt
        {
            get { return _keys.Alt; }
            set { _keys.Alt = value; }
        }

        private IFastBitmap Bitmap { get; set; }

        public bool BlinkEnabled { get; set; }

        private bool Blinking { get; set; }

        private bool Control
        {
            get { return _keys.Control; }
            set { _keys.Control = value; }
        }

        public int ScaleX { get; private set; }

        public int ScaleY { get; private set; }

        private bool Shift
        {
            get { return _keys.Shift; }
            set { _keys.Shift = value; }
        }

        public void AttachKeyHandler(Form form)
        {
            form.KeyDown += (sender, e) => { OnKey(e); };
            form.KeyUp += (sender, e) => { OnKey(e); };
        }

        public bool CursorEnabled { get; set; }

        public int CursorX
        {
            get { return _cursorX; }
            set
            {
                _cursorX = value;
                UpdateCursor(_cursorX, _cursorY);
            }
        }

        public int CursorY
        {
            get { return _cursorY; }
            set
            {
                _cursorY = value;
                UpdateCursor(_cursorX, _cursorY);
            }
        }

        public Bitmap RenderAll()
        {
            return Bitmap.CloneAsBitmap();
        }

        public Bitmap RenderSingle(int character, int color)
        {
            color = TranslateColorIndex(color);
            var result = _terminalFont.RenderUnscaled(character, _terminalPalette[color & 0xF],
                _terminalPalette[(color >> 4) & 0xF]);
            if (_terminalWide)
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

        public IKeyboard Keyboard => _keys;

        public void SetScale(int xScale, int yScale)
        {
            ScaleX = xScale;
            ScaleY = yScale;
            SetSize(_terminalWidth, _terminalHeight, _terminalWide);
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
            if (Bitmap != null)
            {
                Redraw();
            }
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

        void ITerminal.SetSize(int width, int height, bool wide)
        {
            Invoke(new SetSizeDelegate(SetSize), width, height, wide);
        }

        public void Write(int x, int y, string value, int color)
        {
            var ac = new AnsiChar {Color = color};
            var characters = CodePage437.GetBytes(value);
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
            SuspendLayout();
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
            ResumeLayout();
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            if (_updated)
            {
                _updated = false;
                Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            timerDaemon.Dispose();
            base.Dispose(disposing);
        }

        public void Draw(int x, int y, AnsiChar ac)
        {
            if (x >= 0 && x < _terminalWidth && y >= 0 && y < _terminalHeight)
            {
                var newHeight = _terminalFont.Height;
                var newWidth = _terminalFont.Width;
                var newX = x*newWidth;
                var newY = y*newHeight;
                var backColor = _terminalPalette.Colors[(ac.Color >> 4) & (BlinkEnabled ? 0x7 : 0xF)];
                var foreColor = BlinkEnabled && Blinking && (ac.Color & 0x80) != 0
                    ? backColor
                    : _terminalPalette.Colors[ac.Color & 0xF];
                _terminalFont.Render(Bitmap, newX, newY, ac.Char, foreColor, backColor);
                _updated = true;
            }
        }

        private void Initialize()
        {
            _keys = new KeysBuffer();
            CursorEnabled = false;
            InitializeComponent();
            Load += OnLoad;
            ScaleX = 1;
            ScaleY = 1;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, true);
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

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            _keys.Press(e.KeyChar);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            _terminalFont = new RasterFont();
            _terminalPalette = new Palette();
            SetSize(80, 25, false);
            BlinkEnabled = true;
            timerDaemon.Start(Blink, 1f/0.2f);
            displayTimer.Enabled = true;
        }

        private void OnMouse(object sender, MouseEventArgs e)
        {
            if (CursorEnabled)
            {
                var newX = e.X/_terminalFont.Width;
                var newY = e.Y/_terminalFont.Height;
                UpdateCursor(newX, newY);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouse(this, e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            OnMouse(this, e);
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Bitmap != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                e.Graphics.SmoothingMode = SmoothingMode.None;
                e.Graphics.DrawImageUnscaled((FastBitmap) Bitmap, 0, 0);
                UpdateCursor();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
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

        public void Redraw()
        {
            var index = 0;
            for (var y = 0; y < _terminalHeight; y++)
            {
                for (var x = 0; x < _terminalWidth; x++)
                {
                    Draw(x, y, _terminalBuffer[index++]);
                }
            }
        }

        public void SetSize(int width, int height, bool wide)
        {
            if (width == 0 || height == 0)
                return;

            var oldWidth = _terminalWidth;
            var oldHeight = _terminalHeight;
            _terminalWidth = width;
            _terminalHeight = height;

            _terminalFont.SetScale((wide ? 2 : 1)*ScaleX, ScaleY);
            _terminalWide = wide;

            var oldBitmap = Bitmap;
            Bitmap = new FastBitmap(_terminalFont.Width*_terminalWidth*(_terminalWide ? 2 : 1),
                _terminalFont.Height*_terminalHeight);
            oldBitmap?.Dispose();
            Array.Resize(ref _terminalBuffer, _terminalWidth*_terminalHeight);

            if (width != oldWidth || height != oldHeight)
            {
                Clear();
            }

            if (AutoSize)
            {
                Width = _terminalFont.Width*_terminalWidth;
                Height = _terminalFont.Height*_terminalHeight;
            }

            Redraw();
        }

        private int TranslateColorIndex(int color)
        {
            // if blinking is enabled, we only get the first 8 colors for background
            if (BlinkEnabled)
            {
                color &= 0x7F;
            }
            return color;
        }

        private void UpdateCursor()
        {
            if (CursorEnabled)
            {
                using (var g = CreateGraphics())
                {
                    using (
                        Pen bright = new Pen(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD)),
                            dark = new Pen(Color.FromArgb(0xFF, 0x22, 0x22, 0x22)))
                    {
                        var outerRect = new Rectangle(_cursorX*_terminalFont.Width, _cursorY*_terminalFont.Height,
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
        }

        private void UpdateCursor(int newX, int newY)
        {
            if (Shift && !_shiftHoldX && !_shiftHoldY)
            {
                if (_cursorX == newX && _cursorY != newY)
                    _shiftHoldX = true;
                else if (_cursorX != newX && _cursorY == newY)
                    _shiftHoldY = true;
            }

            if (_shiftHoldX)
            {
                newX = _cursorX;
            }

            if (_shiftHoldY)
            {
                newY = _cursorY;
            }

            if (newX != _cursorX || newY != _cursorY)
            {
                _updated = true;
                _cursorX = newX;
                _cursorY = newY;
                UpdateCursor();
            }
        }

        private delegate void SetSizeDelegate(int width, int height, bool wide);
    }
}