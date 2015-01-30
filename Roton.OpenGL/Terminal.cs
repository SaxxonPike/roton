using Roton.Windows;
using System;
using System.Drawing;
using System.Drawing.Imaging;
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
        private Windows.Font _terminalFont;
        private int _terminalHeight;
        private Windows.Palette _terminalPalette;
        private int _terminalWidth;
        private bool _updated;
        private bool _wideMode;

        public Terminal()
        {
            _keys = new KeysBuffer();

            InitializeComponent();

            // Initialize font and palette.
            _terminalFont = new Windows.Font();
            _terminalPalette = new Palette();
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

        public void ClearKeys() {
            _keys.Clear();
        }

        private FastBitmap Bitmap { get; set; }

        public IKeyboard Keyboard {
            get { return _keys as IKeyboard; }
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

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
            _keys.Press(e.KeyChar);
        }

        public bool Shift {
            get { return _keys.Shift; }
            set { _keys.Shift = value; }
        }

        public bool Control {
            get { return _keys.Control; }
            set { _keys.Control = value; }
        }

        public void Clear()
        {
            _terminalBuffer = new AnsiChar[_terminalWidth * _terminalHeight];
        }

        private void Draw(int x, int y, AnsiChar ac)
        {
            if ((x >= 0 && x < _terminalWidth) && (y >= 0 && y < _terminalHeight))
            {
                int drawX = x * _terminalFont.Width;
                int drawY = y * _terminalFont.Height;
                int fgColor = _terminalPalette.Colors[(ac.Color >> 4)];
                int bgColor = _terminalPalette.Colors[ac.Color & 0xF];
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
            int glNewTexture = GL.GenTexture();

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
        
        public void Plot(int x, int y, AnsiChar ac)
        {
            if ((x >= 0 && x < _terminalWidth) && (y >= 0 && y < _terminalHeight))
            {
                int index = x + (y * _terminalWidth);
                _terminalBuffer[index] = ac;
                Draw(x, y, ac);
            }
        }

        private void Redraw()
        {
            int index = 0;
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
            SetSize(_terminalWidth, _terminalHeight, _wideMode);
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

        public void Write(int x, int y, string value, int color)
        {
            AnsiChar ac = new AnsiChar();
            ac.Color = color;
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

            // Finish setting up the control and start the rendering timer.
            SetSize(80, 25, false);
            displayTimer.Enabled = true;
        }
    }
}
