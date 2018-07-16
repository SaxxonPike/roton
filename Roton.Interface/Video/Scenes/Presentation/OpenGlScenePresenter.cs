using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Roton.Interface.Video.Scenes.Composition;

namespace Roton.Interface.Video.Scenes.Presentation
{
    /// <summary>
    /// Basic OpenGL 3.0 renderer. This sceneComposer does not support shaders or
    /// anything as long as it supports frame buffer objects (FBOs).
    /// </summary>
    public class OpenGlScenePresenter : IOpenGlScenePresenter
    {
        private readonly Func<IBitmapSceneComposer> _getBitmapSceneComposer;
        private readonly Func<IOpenGlScenePresenterWindow> _getOpenGlWindow;
        private int _glLastTexture = -1;
        private bool _glReady => OpenTK.Graphics.GraphicsContext.CurrentContext != null;
        private bool _initted;

        public OpenGlScenePresenter(IBitmapSceneComposer bitmapSceneComposer, IOpenGlScenePresenterWindow openGlWindow) 
            : this(() => bitmapSceneComposer, () => openGlWindow)
        {
        }

        public OpenGlScenePresenter(Func<IBitmapSceneComposer> getBitmapSceneComposer, Func<IOpenGlScenePresenterWindow> getOpenGlWindow)
        {
            _getBitmapSceneComposer = getBitmapSceneComposer;
            _getOpenGlWindow = getOpenGlWindow;
        }

        /// <summary>
        /// Initializes the OpenGL renderer.
        /// </summary>
        private void Init()
        {
            // If a GLControl is assigned, it must be set as the current context
            // before anything happens.
            _getOpenGlWindow()?.MakeCurrent();
            GL.ClearColor(Color.Black);
            GL.Disable(EnableCap.Lighting); // unnecessary
            GL.Disable(EnableCap.DepthTest); // unnecessary
            GL.Enable(EnableCap.Texture2D); // required for FBOs to work
            _initted = true;
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        public void Render()
        {
            if (!_glReady) return;

            if (!_initted)
                Init();

            var window = _getOpenGlWindow();
            if (window == null)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit);

            var composer = _getBitmapSceneComposer();
            if (composer == null)
                return;

            GenerateTexture(composer.DirectAccessBitmap);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            //GL.Begin(BeginMode.Quads);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(0.0f, 0.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(composer.Columns, 0.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(composer.Columns, composer.Rows);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(0.0f, composer.Rows);
            GL.End();

            window.SwapBuffers();
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
                PixelFormat.Bgra, PixelType.UnsignedByte, gameBitmap.BitsPointer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            if (_glLastTexture != -1)
                GL.DeleteTexture(_glLastTexture);
            _glLastTexture = glNewTexture;
        }

        private void UpdateViewportImplementation()
        {
            var control = _getOpenGlWindow();
            if (control == null)
                return;

            var composer = _getBitmapSceneComposer();
            if (composer == null)
                return;

            GL.Viewport(0, 0, control.Width, control.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, composer.Columns, composer.Rows, 0.0, -1.0, 1.0);
        }
    }
}
