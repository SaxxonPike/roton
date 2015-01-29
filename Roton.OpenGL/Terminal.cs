using Roton.Windows;
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using OpenTK.Graphics;

namespace Roton.OpenGL {
    public partial class Terminal : UserControl, ITerminal
    {
        private bool _glReady = false;
        private KeysBuffer _keys;
        private bool _shiftHoldX;
        private bool _shiftHoldY;
        private AnsiChar[] _terminalBuffer;
        private Windows.Font _terminalFont;
        private int _terminalHeight;
        private Windows.Palette _terminalPalette;
        private int _terminalWidth;
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

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e) {
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

        public void Plot(int x, int y, AnsiChar ac)
        {
        }

        private void Redraw()
        {
            if(!_glReady) return;

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Color3(Color.IndianRed);
            GL.Begin(BeginMode.Quads);
            GL.Vertex2(8.0, 8.0);
            GL.Vertex2(64.0, 9.0);
            GL.Vertex2(65.0, 20.0);
            GL.Vertex2(8.0, 21.0);
            GL.End();

            glControl.SwapBuffers();
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
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            Redraw();
        }

        private void glControl_Load(object sender, EventArgs e) {
            // Set GLControl to be the active GL context.
            glControl.MakeCurrent();

            // Initialize OpenGL.
            GL.ClearColor(Color.BlueViolet);
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
