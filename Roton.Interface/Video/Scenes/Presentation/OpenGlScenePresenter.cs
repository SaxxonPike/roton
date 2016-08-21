using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Roton.Interface.Video.Scenes.Composition;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Roton.Interface.Video.Scenes.Presentation
{
    /// <summary>
    /// Basic OpenGL 3.0 renderer. This sceneComposer does not support shaders or
    /// anything as long as it supports frame buffer objects (FBOs).
    /// </summary>
    public class OpenGlScenePresenter : IOpenGlScenePresenter
    {
        private int _glLastTexture = -1;
        private bool _glReady;
        public GLControl FormControl { get; set; }
        public double TerminalHeight { get; set; }
        public double TerminalWidth { get; set; }

        /// <summary>
        /// Initializes the OpenGL renderer.
        /// </summary>
        public void Init()
        {
            // If a GLControl is assigned, it must be set as the current context
            // before anything happens.
            FormControl?.MakeCurrent();

            GL.ClearColor(Color.Black);
            GL.Disable(EnableCap.Lighting); // unnecessary
            GL.Disable(EnableCap.DepthTest); // unnecessary
            GL.Enable(EnableCap.Texture2D); // required for FBOs to work
            _glReady = true;
        }

        /// <summary>
        /// Renders the scene. If <see cref="FormControl" /> is assigned, its
        /// buffer will be swapped on each render call.
        /// </summary>
        /// <param name="composer">Composer to obtain the bitmap data from.</param>
        public void Render(IBitmapSceneComposer composer)
        {
            if (!_glReady) return;

            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Don't draw anything if the Bitmap is null.
            if (composer == null) return;

            GenerateTexture(composer.DirectAccessBitmap);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(0.0f, 0.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(TerminalWidth, 0.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(TerminalWidth, TerminalHeight);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(0.0f, TerminalHeight);
            GL.End();

            FormControl?.SwapBuffers();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateViewport()
        {
            if (!_glReady) return;

            UpdateViewportImplementation();
        }

        private void GenerateTexture(IDirectAccessBitmap gameBitmap) {
            if(gameBitmap == null) return;

            var glNewTexture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, glNewTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, gameBitmap.Width, gameBitmap.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, gameBitmap.BitsPointer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            if(_glLastTexture != -1)
                GL.DeleteTexture(_glLastTexture);
            _glLastTexture = glNewTexture;
        }

        private void UpdateViewportImplementation()
        {
            GL.Viewport(0, 0, FormControl.Width, FormControl.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, TerminalWidth, TerminalHeight, 0.0, -1.0, 1.0);
        }
    }
}
