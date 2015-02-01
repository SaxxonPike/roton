using Roton.Common;
using Roton.Windows;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.AccessControl;
using WinPixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Text;
using OpenTK.Graphics.OpenGL;
using GLPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using System.Windows.Forms;

namespace Roton.OpenGL {
    public partial class Terminal : UserControl, ITerminal
    {
        static private Encoding _encoding = Encoding.GetEncoding(437);

        private bool _glReady = false;
        private int _glLastTexture = -1;
        private KeysBuffer _keys;
        private bool _shiftHoldX;
        private bool _shiftHoldY;
        private AnsiChar[] _terminalBuffer;
        private Common.Font _terminalFont;
        private int _terminalHeight;
        private Common.Palette _terminalPalette;
        private int _terminalWidth;
        private bool _updated;
        private bool _wideMode;

        public Terminal()
        {
            _keys = new KeysBuffer();

            InitializeComponent();

            // Initialize font and palette.
            _terminalFont = new Common.Font();
            _terminalPalette = new Common.Palette();

            // Set default scale.
            ScaleX = 1;
            ScaleY = 1;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Alt {
            get { return _keys.Alt; }
            set { _keys.Alt = value; }
        }

        public void AttachKeyHandler(Form form) {
            form.KeyDown += (object sender, KeyEventArgs e) => { OnKey(e); };
            form.KeyUp += (object sender, KeyEventArgs e) => { OnKey(e); };
        }

        public bool BlinkEnabled {
            get;
            set;
        }

        bool Blinking {
            get;
            set;
        }

        public void ClearKeys() {
            _keys.Clear();
        }

        private FastBitmap Bitmap { get; set; }

        public IKeyboard Keyboard {
            get { return _keys as IKeyboard; }
        }

        public bool Shift {
            get { return _keys.Shift; }
            set { _keys.Shift = value; }
        }

        public bool Control {
            get { return _keys.Control; }
            set { _keys.Control = value; }
        }

        void Blink() {
            SuspendLayout();
            Blinking = !Blinking;
            if(_terminalWidth > 0 && _terminalHeight > 0 && (Bitmap != null)) {
                int total = _terminalWidth * _terminalHeight;
                int i = 0;
                for(int y = 0; y < _terminalHeight; y++) {
                    for(int x = 0; x < _terminalWidth; x++) {
                        if((_terminalBuffer[i].Color & 0x80) != 0) {
                            Draw(x, y, _terminalBuffer[i]);
                        }
                        i++;
                    }
                }
            }
            ResumeLayout();
        }

        public void Clear()
        {
            _terminalBuffer = new AnsiChar[_terminalWidth * _terminalHeight];
        }

        private void Draw(int x, int y, AnsiChar ac)
        {
            if ((x >= 0 && x < _terminalWidth) && (y >= 0 && y < _terminalHeight))
            {
                var drawX = x * _terminalFont.Width;
                var drawY = y * _terminalFont.Height;
                var bgColor = _terminalPalette.Colors[(ac.Color >> 4) & (BlinkEnabled ? 0x7 : 0xF)];
                var fgColor = (BlinkEnabled && Blinking && (ac.Color & 0x80) != 0) ? bgColor : _terminalPalette.Colors[ac.Color & 0xF];
                _terminalFont.Render(Bitmap, drawX, drawY, ac.Char, fgColor, bgColor);
                _updated = true;                
            }
        }

        private void GlRender() {
            if(!_glReady) return;

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GlGenerateTexture();

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(0.0f, 0.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(_terminalWidth, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(_terminalWidth, _terminalHeight);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(0.0f, _terminalHeight);
            GL.End();

            glControl.SwapBuffers();
        }

        private void GlGenerateTexture()
        {
            var glNewTexture = GL.GenTexture();

            BitmapData fbData = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadOnly, WinPixelFormat.Format32bppArgb);
            GL.BindTexture(TextureTarget.Texture2D, glNewTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Bitmap.Width, Bitmap.Height, 0, GLPixelFormat.Bgra, PixelType.UnsignedByte, fbData.Scan0);
            Bitmap.UnlockBits(fbData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            if (_glLastTexture != -1)
                GL.DeleteTexture(_glLastTexture);
            _glLastTexture = glNewTexture;
        }

        void OnKey(KeyEventArgs e) {
            if(!e.Shift) {
                Shift = false;
                _shiftHoldX = false;
                _shiftHoldY = false;
            } else {
                Shift = true;
            }
            Alt = e.Alt;
            Control = e.Control;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            var keyAlt = (keyData & Keys.Alt) != 0;
            var keyControl = (keyData & Keys.Control) != 0;
            var keyRaw = (keyData & Keys.KeyCode);
            _keys.NumLock = IsKeyLocked(Keys.NumLock);
            _keys.CapsLock = IsKeyLocked(Keys.CapsLock);

            bool result = base.ProcessCmdKey(ref msg, keyData);
            if(!keyAlt && !keyControl && keyRaw != Keys.ShiftKey) {
                OnKey(new KeyEventArgs(keyData));
                return _keys.Press(keyData);
            }
            return result;
        }
        
        public void Plot(int x, int y, AnsiChar ac)
        {
            if ((x >= 0 && x < _terminalWidth) && (y >= 0 && y < _terminalHeight))
            {
                var index = x + (y * _terminalWidth);
                _terminalBuffer[index] = ac;
                Draw(x, y, ac);
            }
        }

        private void Redraw()
        {
            var index = 0;
            for(var y = 0; y < _terminalHeight; y++)
                for (var x = 0; x < _terminalWidth; x++)
                    Draw(x, y, _terminalBuffer[index++]);
        }

        public int ScaleX {
            get;
            private set;
        }

        public int ScaleY {
            get;
            private set;
        }

        public void SetScale(int xScale, int yScale)
        {
            ScaleX = xScale;
            ScaleY = yScale;

            if (AutoSize)
            {
                Width = (_terminalWidth * _terminalFont.Width) * xScale;
                Height = (_terminalHeight * _terminalFont.Height) * yScale;
            }

            SetViewport();
        }

        public void SetSize(int width, int height, bool wide)
        {
            var oldWidth = _terminalWidth;
            var oldHeight = _terminalHeight;
            _terminalWidth = width;
            _terminalHeight = height;
            _wideMode = wide;

            // Ignore wide mode with bitmaps; all scaling will be handled by the GPU.
            var oldBitmap = Bitmap;
            Bitmap = new FastBitmap(_terminalWidth * _terminalFont.Width, _terminalHeight * _terminalFont.Height);
            if (oldBitmap != null)
                oldBitmap.Dispose();

            if (width != oldWidth || height != oldHeight)
                Clear();

            if (AutoSize)
            {
                Width = _terminalWidth * _terminalFont.Width * ScaleX;
                Height = _terminalHeight * _terminalFont.Height * ScaleY;
            }

            // Reconfigure OpenGL viewport.
            SetViewport();
        }

        private void SetViewport()
        {
            if (!_glReady) return;

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, _terminalWidth, _terminalHeight, 0.0, -1.0, 1.0);
        }

        public Common.Font TerminalFont {
            get { return _terminalFont; }
            set {
                _terminalFont = value;
                Redraw();
            }
        }

        public Common.Palette TerminalPalette {
            get { return _terminalPalette; }
            set {
                _terminalPalette = value;
                Redraw();
            }
        }

        public void Write(int x, int y, string value, int color)
        {
            var ac = new AnsiChar {Color = color};
            var characters = _encoding.GetBytes(value);
            var count = characters.Length;

            while(x < 0) { x += _terminalWidth; y--; }

            for(int index = 0; index < count; index++) {
                ac.Char = characters[index];
                Plot(x, y, ac);
                x++;
                if(x >= _terminalWidth) { x -= _terminalWidth; y++; }
            }
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            Redraw();
            GlRender();
        }

        private void glControl_Load(object sender, EventArgs e) {
            // Set up key events.
            glControl.KeyPress += glControl_KeyPress;

            // Set GLControl to be the active GL context.
            glControl.MakeCurrent();

            // Initialize OpenGL.
            GL.ClearColor(Color.Black);
            GL.Disable(EnableCap.Lighting);  // unnecessary
            GL.Disable(EnableCap.DepthTest); // unnecessary
            GL.Enable(EnableCap.Texture2D);  // required for FBOs to work
            GL.Ortho(0.0, _terminalWidth, _terminalHeight, 0.0, -1.0, 1.0);

            _glReady = true;
            SetViewport();

            // Enable blinking by default; set timer for it.
            BlinkEnabled = true;
            timerDaemon.Start(Blink, 1f / 0.2f);

            // Finish setting up the control and start the rendering timer.
            SetSize(80, 25, false);
            displayTimer.Enabled = true;
        }

        void glControl_KeyPress(object sender, KeyPressEventArgs e) {
            base.OnKeyPress(e);
            _keys.Press(e.KeyChar);
        }
    }
}
